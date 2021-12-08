using System;
using System.IO;
using System.Text;
using System.Threading;
using Backups.Entities.Files;
using Backups.Enums;
using BackupsExtra.Algorithms.CleaningAlgorithms;
using BackupsExtra.Algorithms.ConfigAlgorithms;
using BackupsExtra.Builders;
using BackupsExtra.Entities;
using BackupsExtra.Enums;
using BackupsExtra.Loggers;
using BackupsExtra.Services;
using BackupsExtra.Tools;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class Tests
    {
        private const string FilePath1 = "FileA.txt";
        private const string FilePath2 = "FileB.exe";
        
        private const string WithoutExtensions1 = "FileA";

        private BackupService _backupService;

        [SetUp]
        public void Setup()
        {
            File.Create(FilePath1).Close();
            File.Create(FilePath2).Close();

            _backupService = new BackupService();
        }

        [TearDown]
        public void Teardown()
        {
            File.Delete(FilePath1);
            File.Delete(FilePath2);

            DirectoryInfo[] directories = new DirectoryInfo(
                AppDomain.CurrentDomain.BaseDirectory)
                .GetDirectories("*DirectoryName*");

            foreach (DirectoryInfo directoryInfo in directories)
            {
                if (Directory.Exists(directoryInfo.Name))
                    Directory.Delete(directoryInfo.Name, true);
            }
        }

        [Test]
        public void ClearRestorePointsByCount()
        {
            const int maxNumberOfPoints = 2;
            const int desirableNumberOfPoints = 5;

            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName1")
                    .SetStorageType(StorageTypes.SplitStorage)
                    .SetLogger(new ConsoleLogger())
                    .SetCleaningAlgorithm(new CountCleaningAlgorithm(maxNumberOfPoints)));

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            for (int i = 0; i < desirableNumberOfPoints; i++)
            {
                backupJob.CreateRestorePoint();
            }

            Assert.That(backupJob.RestorePointsCount is maxNumberOfPoints);
        }

        [Test]
        public void ClearRestorePointsByTime()
        {
            var timeToBeStored = TimeSpan.FromDays(30);

            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName2")
                    .SetStorageType(StorageTypes.SingleStorage)
                    .SetLogger(new ConsoleLogger())
                    .SetCleaningAlgorithm(
                        new TimeCleaningAlgorithm(timeToBeStored))
                    .Build());

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            backupJob.CreateRestorePoint();

            var timeOfCreation = new DateTime(2020, 12, 31);
            backupJob.CreateRestorePoint(timeOfCreation);

            backupJob.CreateRestorePoint();

            Assert.That(backupJob.RestorePointsCount is 2);
        }

        [Test]
        public void CreateRestorePointsByHybirdAlgorithm()
        {
            AbstractCleaningAlgorithm cleaningAlgorithm =
                new HybridCleaningAlgorithm(
                    CleaningConditions.DoesNotFitAllLimits, 
                    new CountCleaningAlgorithm(2),
                    new TimeCleaningAlgorithm(TimeSpan.FromDays(1)));

            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName3")
                    .SetStorageType(StorageTypes.SingleStorage)
                    .SetLogger(new ConsoleLogger())
                    .SetCleaningAlgorithm(cleaningAlgorithm));

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            backupJob.CreateRestorePoint();

            var timeOfCreation = new DateTime(2021, 11, 25);
            backupJob.CreateRestorePoint(timeOfCreation);

            backupJob.CreateRestorePoint();

            backupJob.CreateRestorePoint(timeOfCreation);

            Assert.That(backupJob.RestorePointsCount is 3);
        }

        [Test]
        public void MergeByCount_MergedFileTimeIsLesserThanRestorePoints()
        {
            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName4")
                    .SetStorageType(StorageTypes.SplitStorage)
                    .SetLimitBehavior(LimitBehavior.Merge)
                    .SetLogger(new ConsoleLogger())
                    .SetCleaningAlgorithm(new CountCleaningAlgorithm(1)));

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            backupJob.CreateRestorePoint();
            Thread.Sleep(500);
            
            backupJob.DeleteJobObjects(FilePath1);

            RestorePoint remainingPoint = backupJob.CreateRestorePoint();

            Assert.That(backupJob.RestorePointsCount is 1);
            Assert.That(remainingPoint.JobObjects.Count is 2);

            string newFileName = new Storage("DirectoryName4", 2).FullPath(WithoutExtensions1);
            Console.WriteLine(newFileName);
            Assert.That(File.Exists(newFileName));
            Assert.Less(File.GetCreationTime(newFileName), remainingPoint.CreationTime);
        }

        [Test]
        public void FileLoggerRecordingLog()
        {
            const string logFilePath = "log.txt";

            string logText =
                new StringBuilder()
                    .AppendLine("Created backup job")
                    .AppendLine("Added job object: FileA.txt")
                    .AppendLine("Created restore point number 1")
                    .ToString();

            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName5")
                    .SetStorageType(StorageTypes.SingleStorage)
                    .SetLogger(new FileLogger()));

            backupJob.AddJobObjects(new JobObject(FilePath1));
            backupJob.CreateRestorePoint();

            Assert.That(File.Exists(logFilePath));
            
            Assert.AreEqual(logText, File.ReadAllText(logFilePath));

            File.Delete(logFilePath);
        }

        [Test]
        public void SystemLoadedFromSave_InformationRemained()
        {
            const string configPath = ".config";

            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName6")
                    .SetStorageType(StorageTypes.SplitStorage)
                    .SetLogger(new ConsoleLogger())
                    .SetCleaningAlgorithm(new CountCleaningAlgorithm(5)));

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            backupJob.CreateRestorePoint();

            backupJob.DeleteJobObjects(FilePath2);

            backupJob.CreateRestorePoint();

            _backupService.Save();

            BackupService backupService = new ConfigHandler().Load();

            BackupJob loadedBackupJob = backupService.GetBackupJob(0);

            Assert.AreEqual("DirectoryName6", loadedBackupJob.DirectoryPath);
            Assert.AreEqual(StorageTypes.SplitStorage, loadedBackupJob.StorageType);

            Assert.That(loadedBackupJob.RestorePointsCount is 2);

            Assert.AreEqual(FilePath1, loadedBackupJob.GetJobObject(FilePath1).Name);
            Assert.IsNull(loadedBackupJob.GetJobObject(FilePath2));

            File.Delete(configPath);
        }

        [Test]
        public void RestoreFromRestorePoint_FilesExistAfterRestoration()
        {
            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName7")
                    .SetStorageType(StorageTypes.SplitStorage)
                    .SetLogger(new ConsoleLogger())
                    .SetCleaningAlgorithm(new CountCleaningAlgorithm(5)));

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            RestorePoint restorePoint = backupJob.CreateRestorePoint();

            backupJob.DeleteJobObjects(FilePath1);

            Assert.IsFalse(File.Exists(FilePath1));
            Assert.IsTrue(File.Exists(FilePath2));

            restorePoint.Restore();
            
            Assert.IsTrue(File.Exists(FilePath1));
            Assert.IsTrue(File.Exists(FilePath2));
        }

        [Test]
        public void RestoreFromRestorePointToNewDirectory_FilesExist()
        {
            const string restoredDirectoryPath = "DirectoryToRestore";

            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName8")
                    .SetStorageType(StorageTypes.SingleStorage)
                    .SetLogger(new ConsoleLogger())
                    .SetCleaningAlgorithm(new CountCleaningAlgorithm(5)));

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            RestorePoint restorePoint = backupJob.CreateRestorePoint();
            restorePoint.Restore(restoredDirectoryPath);

            Assert.IsTrue(
                File.Exists(
                    Path.Combine(
                        restoredDirectoryPath,
                        FilePath1)));

            Assert.IsTrue(
                File.Exists(
                    Path.Combine(
                        restoredDirectoryPath,
                        FilePath2)));

            Directory.Delete(restoredDirectoryPath, true);
        }

        [Test]
        public void CannotInitializeBackupJobWithoutStoragePath_ThrowException()
        {
            Assert.Catch<BackupsExtraException>(() =>
            {
                new BackupJobBuilder()
                    .SetDirectoryPath("DirectoryName9")
                    .Build();
            });
        }

        [Test]
        public void CannotCreateRestorePointWithNoFiles_ThrowException()
        {
            BackupJob backupJob = _backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetStorageType(StorageTypes.SplitStorage));

            Assert.Catch<BackupsExtraException>(() =>
            {
                backupJob.CreateRestorePoint();
            });
        }
    }
}
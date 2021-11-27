using System;
using System.IO;
using Backups.Entities.Files;
using Backups.Enums;
using BackupsExtra.Algorithms.CleaningAlgorithms;
using BackupsExtra.Algorithms.ConfigAlgorithms;
using BackupsExtra.Builders;
using BackupsExtra.Entities;
using BackupsExtra.Enums;
using BackupsExtra.Loggers;
using BackupsExtra.Services;

namespace BackupsExtra
{
    internal class Program
    {
        private const string DirectoryName = "Backups";

        private const string FilePath1 = "FileA.txt";
        private const string FilePath2 = "FileB.exe";

        private const string ConfigPath = ".config";

        private static void Main()
        {
            File.Create(FilePath1).Close();
            File.Create(FilePath2).Close();

            LaunchSystem();
            LaunchSystemFromConfig();

            File.Delete(ConfigPath);

            File.Delete(FilePath1);
            File.Delete(FilePath2);

            Directory.Delete(DirectoryName, true);
        }

        private static void LaunchSystem()
        {
            var backupService = new BackupService();

            BackupJob backupJob = backupService
                .AddBackupJob(
                new BackupJobBuilder()
                    .SetDirectoryPath(DirectoryName)
                    .SetStorageType(StorageTypes.SplitStorage)
                    .SetLogger(new ConsoleLogger())
                    .SetLimitBehavior(LimitBehavior.Merge)
                    .SetCleaningAlgorithm(new CountCleaningAlgorithm(2)));

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            Console.ReadKey();

            backupJob.CreateRestorePoint();

            Console.ReadKey();

            backupJob.DeleteJobObjects(FilePath2);

            Console.ReadKey();

            backupJob.CreateRestorePoint();

            backupService.Save();

            Console.ReadKey();
        }

        private static void LaunchSystemFromConfig()
        {
            BackupService loadedbackupService = new ConfigHandler().Load();

            BackupJob loadedBackupJob = loadedbackupService.GetBackupJob(0);

            loadedBackupJob.CreateRestorePoint();

            Console.ReadKey();

            loadedBackupJob.GetRestorePoint(2).Restore();
            loadedBackupJob.DeleteRestorePoint(2);

            Console.ReadKey();
        }
    }
}

using System.IO;
using Backups.Entities;
using Backups.Entities.Files;
using Backups.Enums;
using NUnit.Framework;

namespace Backups.Tests
{
    [TestFixture]
    public class Tests
    {
        private const string FilePath1 = "FileA.txt";
        private const string FilePath2 = "FileB.exe";
        private const string FilePath3 = "FileC.abu.zxc";

        private const string WithoutExtensions1 = "FileA";
        private const string WithoutExtensions2 = "FileB";
        private const string WithoutExtensions3 = "FileC.abu";

        [Test]
        public void CreateSplitStorage()
        {
            File.Create(FilePath1).Close();
            File.Create(FilePath2).Close();
            
            var backupJob = new BackupJob(StorageTypes.SplitStorage);
            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            backupJob.CreateRestorePoint();

            backupJob.DeleteJobObject(FilePath2);

            backupJob.CreateRestorePoint();

            Assert.True(backupJob.RestorePointsCount == 2);

            var storage1 = new Storage(1);

            string storageA1FullPath = storage1.FullPath(WithoutExtensions1);
            string storageB1FullPath = storage1.FullPath(WithoutExtensions2);

            Assert.True(File.Exists(storageA1FullPath));
            Assert.True(File.Exists(storageB1FullPath));

            var storage2 = new Storage(2);

            string storageA2FullPath = storage2.FullPath(WithoutExtensions1);
            string storageB2FullPath = storage2.FullPath(WithoutExtensions2);

            Assert.True(File.Exists(storageA2FullPath));
            Assert.False(File.Exists(storageB2FullPath));

            File.Delete(FilePath1);
            File.Delete(FilePath2);
            File.Delete(storageA1FullPath);
            File.Delete(storageB1FullPath);
            File.Delete(storageA2FullPath);
        }
    }
}
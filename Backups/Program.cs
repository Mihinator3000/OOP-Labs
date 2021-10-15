using System;
using System.IO;
using Backups.Entities;
using Backups.Entities.Files;
using Backups.Enums;

namespace Backups
{
    internal class Program
    {
        private const string DirectoryPath = "Backups";
        private const string FilePath1 = "FileA.txt";
        private const string FilePath2 = "FileB.exe";

        private static void Main()
        {
            File.Create(FilePath1).Close();
            File.Create(FilePath2).Close();

            CreateSingleStorage();
            Console.ReadKey();

            File.Delete(FilePath1);
            File.Delete(FilePath2);
            Directory.Delete(DirectoryPath, true);
        }

        private static void CreateSingleStorage()
        {
            var backupJob = new BackupJob(DirectoryPath, StorageTypes.SingleStorage);

            backupJob.AddJobObjects(
                new JobObject(FilePath1),
                new JobObject(FilePath2));

            backupJob.CreateRestorePoint();
        }
    }
}

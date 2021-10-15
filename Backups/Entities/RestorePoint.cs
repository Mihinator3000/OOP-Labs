using System;
using System.Collections.Generic;
using System.IO;
using Backups.Algorithms;
using Backups.Entities.Files;
using Backups.Enums;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private readonly StorageTypes _storageType;
        private readonly int _number;

        public RestorePoint(List<JobObject> jobObjects, StorageTypes storageType, int number)
        {
            JobObjects = jobObjects;
            _storageType = storageType;
            _number = number;
        }

        public DateTime CreationTime { get; private set; }

        public List<JobObject> JobObjects { get; }

        public void Create(string directoryPath)
        {
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
            }

            ResolveStorageType(_storageType)
                .Create(JobObjects, new Storage(directoryPath, _number));

            CreationTime = DateTime.Now;
        }

        private static IStorageAlgorithm ResolveStorageType(StorageTypes storageType)
        {
            return storageType switch
            {
                StorageTypes.SplitStorage =>
                    new SplitStorageAlgorithm(),

                StorageTypes.SingleStorage =>
                    new SingleStorageAlgorithm(),

                _ => throw new BackupsException(
                    storageType.ToString())
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Backups.Algorithms;
using Backups.Entities.Files;
using Backups.Enums;
using Backups.Tools;

namespace Backups.Entities
{
    [DataContract]
    public class RestorePoint : AbstractRestorePoint
    {
        public RestorePoint(List<AbstractJobObject> jobObjects, StorageTypes storageType, int number)
        {
            JobObjects = jobObjects;
            StorageType = storageType;
            Number = number;
        }

        [DataMember]
        public int Number { get; protected set; }

        [DataMember]
        public override DateTime CreationTime { get; protected set; }

        [DataMember]
        public sealed override List<AbstractJobObject> JobObjects { get; protected set; }

        [DataMember]
        public StorageTypes StorageType { get; protected set; }

        public override void Create(string directoryPath)
        {
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
            }

            ResolveStorageType(StorageType)
                .Create(JobObjects, new Storage(directoryPath, Number));

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

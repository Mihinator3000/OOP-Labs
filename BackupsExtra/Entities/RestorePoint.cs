using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Backups.Entities.Files;
using Backups.Enums;
using BackupsExtra.Algorithms.RestoreAlgorithms;
using BackupsExtra.Loggers;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    [DataContract]
    public class RestorePoint : Backups.Entities.RestorePoint
    {
        private const string TemporaryRepositoryPath = ".temp.repository";

        [DataMember(Name = "Logger")]
        private readonly AbstractLogger _logger;

        [DataMember(Name = "Storage")]
        private Storage _storage;

        internal RestorePoint(List<AbstractJobObject> jobObjects, StorageTypes storageType, int number, AbstractLogger logger)
            : base(jobObjects, storageType, number)
        {
            _logger = logger;
        }

        public override void Create(string directoryPath)
        {
            base.Create(directoryPath);
            _storage = new Storage(directoryPath, Number);
        }

        public void Restore()
        {
            Directory.CreateDirectory(TemporaryRepositoryPath);
            Restore(TemporaryRepositoryPath);

            JobObjects.ForEach(u =>
            {
                string filePath = Path.Combine(TemporaryRepositoryPath, u.Name);
                File.Move(filePath, u.Path, true);
            });

            Directory.Delete(TemporaryRepositoryPath);
        }

        public void Restore(string extractionPath)
        {
            if (!Directory.Exists(_storage.DirectoryPath))
                throw new BackupsExtraException($"Storage does not exist {_storage.DirectoryPath}");

            ResolveResolveType(StorageType)
                .Restore(JobObjects, _storage, extractionPath);

            _logger.Log($"Point number {Number} restored");
        }

        private static IRestoreAlgorithm ResolveResolveType(StorageTypes storageType)
        {
            return storageType switch
            {
                StorageTypes.SplitStorage =>
                    new SplitRestoreAlgorithm(),

                StorageTypes.SingleStorage =>
                    new SingleRestoreAlgorithm(),

                _ => throw new BackupsExtraException(
                    storageType.ToString())
            };
        }
    }
}
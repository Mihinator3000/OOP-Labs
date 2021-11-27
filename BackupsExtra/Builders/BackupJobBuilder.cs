using Backups.Enums;
using BackupsExtra.Algorithms.CleaningAlgorithms;
using BackupsExtra.Entities;
using BackupsExtra.Enums;
using BackupsExtra.Loggers;
using BackupsExtra.Tools;

namespace BackupsExtra.Builders
{
    public class BackupJobBuilder
    {
        private StorageTypes _storageType;
        private LimitBehavior _limitBehavior;
        private string _directoryPath;
        private AbstractLogger _logger;
        private AbstractCleaningAlgorithm _cleaningAlgorithm;

        public BackupJobBuilder SetStorageType(StorageTypes storageType)
        {
            _storageType = storageType;
            return this;
        }

        public BackupJobBuilder SetDirectoryPath(string path)
        {
            _directoryPath = path;
            return this;
        }

        public BackupJobBuilder SetLogger(AbstractLogger logger)
        {
            _logger = logger;
            return this;
        }

        public BackupJobBuilder SetCleaningAlgorithm(AbstractCleaningAlgorithm cleaningAlgorithm)
        {
            _cleaningAlgorithm = cleaningAlgorithm;
            return this;
        }

        public BackupJobBuilder SetLimitBehavior(LimitBehavior limitBehavior)
        {
            _limitBehavior = limitBehavior;
            return this;
        }

        public BackupJob Build()
        {
            if (_storageType == 0)
                throw new BackupsExtraException("Cannot initialize backup job without storage type");

            if (_limitBehavior == 0)
                _limitBehavior = LimitBehavior.Delete;

            return new BackupJob(
                _directoryPath,
                _storageType,
                _logger,
                _cleaningAlgorithm,
                _limitBehavior);
        }
    }
}
using Backups.Enums;
using BackupsExtra.Entities;
using BackupsExtra.Loggers;
using BackupsExtra.Tools;

namespace BackupsExtra.Builders
{
    public class BackupJobBuilder
    {
        private StorageTypes _storageType;
        private string _directoryPath;
        private AbstractLogger _logger;

        public BackupJobBuilder()
        {
        }

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

        public BackupJob Build()
        {
            if (_storageType == 0)
                throw new BackupsExtraException("Cannot initialize backup job without storage path");

            return new BackupJob(
                _directoryPath,
                _storageType,
                _logger ?? throw new BackupsExtraException("Logger is not set"));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Backups.Entities.Files;
using Backups.Enums;
using BackupsExtra.Loggers;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    [DataContract]
    public class BackupJob
    {
        [DataMember(Name = "OldBackupJob")]
        private readonly Backups.Entities.BackupJob _backupJob;

        [DataMember(Name = "Logger")]
        private readonly AbstractLogger _logger;

        internal BackupJob(string path, StorageTypes storageType, AbstractLogger logger)
        {
            _backupJob = new Backups.Entities.BackupJob(path, storageType);
            _logger = logger;
        }

        public string DirectoryPath =>
            _backupJob.DirectoryPath;

        public StorageTypes StorageType =>
            _backupJob.StorageType;

        public int RestorePointsCount =>
            _backupJob.RestorePointsCount;

        public void AddJobObjects(params AbstractJobObject[] jobObjects)
        {
            _backupJob.AddJobObjects(jobObjects);
            foreach (AbstractJobObject jobObject in jobObjects)
            {
                _logger.Log($"Added job object: {jobObject.Path}");
            }
        }

        public AbstractJobObject GetJobObject(string path)
        {
            return _backupJob.GetJobObject(path);
        }

        public void DeleteJobObjects(params string[] paths)
        {
            try
            {
                _backupJob.DeleteJobObjects(paths);
                foreach (string path in paths)
                {
                    _logger.Log($"Added job object: {path}");
                }
            }
            catch (NullReferenceException e)
            {
                _logger.Log($"Failed to delete job object {e.Message}");
                throw new BackupsExtraException(e.Message);
            }
        }

        public RestorePoint CreateRestorePoint()
        {
            try
            {
                List<AbstractJobObject> jobObjects = _backupJob.GetJobObjects();

                if (jobObjects.Count == 0)
                    throw new BackupsExtraException("No objects to backup");

                var restorePoint = new RestorePoint(
                        jobObjects,
                        StorageType,
                        RestorePointsCount + 1,
                        _logger);

                _backupJob.AddRestorePoint(restorePoint);
                restorePoint.Create(DirectoryPath);
                _logger.Log($"Created restore point number {restorePoint.Number}");
                return restorePoint;
            }
            catch (BackupsExtraException e)
            {
                _logger.Log($"Failed to create restore point: {e.Message}");
                throw new BackupsExtraException(e.Message);
            }
        }
    }
}
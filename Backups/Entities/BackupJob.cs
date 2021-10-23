using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities.Files;
using Backups.Enums;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<IJobObject> _jobObjects = new ();
        private readonly List<IRestorePoint> _restorePoints = new ();

        public BackupJob(StorageTypes storageType)
        {
            StorageType = storageType;
        }

        public BackupJob(string directoryPath, StorageTypes storageType)
            : this(storageType)
        {
            DirectoryPath = directoryPath;
        }

        public string DirectoryPath { get; }

        public StorageTypes StorageType { get; }

        public int RestorePointsCount => _restorePoints.Count;

        public void AddJobObjects(params IJobObject[] jobObjects)
        {
            _jobObjects.AddRange(jobObjects ??
                throw new NullReferenceException(nameof(jobObjects)));
        }

        public IJobObject GetJobObject(string path)
        {
            return _jobObjects.FirstOrDefault(u => u.Path == path);
        }

        public void DeleteJobObject(string path)
        {
            _jobObjects.Remove(GetJobObject(path) ??
                throw new NullReferenceException(path));
        }

        public IRestorePoint CreateRestorePoint()
        {
            if (_jobObjects.Count == 0)
                throw new BackupsException("No objects to backup");

            var restorePoint = new RestorePoint(_jobObjects, StorageType, RestorePointsCount + 1);
            _restorePoints.Add(restorePoint);
            restorePoint.Create(DirectoryPath);
            return restorePoint;
        }
    }
}
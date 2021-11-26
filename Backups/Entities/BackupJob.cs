using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Backups.Entities.Files;
using Backups.Enums;
using Backups.Tools;

namespace Backups.Entities
{
    [DataContract]
    public class BackupJob
    {
        [DataMember(Name = "JobObjects")]
        private readonly List<AbstractJobObject> _jobObjects = new ();

        [DataMember(Name="RestorePoints")]
        private readonly List<AbstractRestorePoint> _restorePoints = new ();

        public BackupJob(StorageTypes storageType)
        {
            StorageType = storageType;
        }

        public BackupJob(string directoryPath, StorageTypes storageType)
            : this(storageType)
        {
            DirectoryPath = directoryPath;
        }

        [DataMember]
        public string DirectoryPath { get; protected set; }

        [DataMember]
        public StorageTypes StorageType { get; protected set; }

        public int RestorePointsCount => _restorePoints.Count;

        public void AddJobObjects(params AbstractJobObject[] jobObjects)
        {
            _jobObjects.AddRange(jobObjects ??
                throw new NullReferenceException(nameof(jobObjects)));
        }

        public AbstractJobObject GetJobObject(string path)
        {
            return _jobObjects.FirstOrDefault(u => u.Path == path);
        }

        public void DeleteJobObjects(params string[] paths)
        {
            foreach (string path in paths)
            {
                _jobObjects.Remove(GetJobObject(path) ??
                    throw new NullReferenceException(path));
            }
        }

        public AbstractRestorePoint CreateRestorePoint()
        {
            if (_jobObjects.Count == 0)
                throw new BackupsException("No objects to backup");

            var restorePoint = new RestorePoint(_jobObjects, StorageType, RestorePointsCount + 1);
            _restorePoints.Add(restorePoint);
            restorePoint.Create(DirectoryPath);
            return restorePoint;
        }

        public void AddRestorePoint(AbstractRestorePoint restorePoint)
        {
            _restorePoints.Add(restorePoint);
        }

        public List<AbstractJobObject> GetJobObjects() =>
            _jobObjects;
    }
}
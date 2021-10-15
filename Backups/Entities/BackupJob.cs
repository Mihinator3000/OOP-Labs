using System.Collections.Generic;
using System.Linq;
using Backups.Entities.Files;
using Backups.Enums;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<JobObject> _jobObjects = new ();
        private readonly List<RestorePoint> _restorePoints = new ();

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

        public void AddJobObjects(params JobObject[] jobObjects)
        {
            _jobObjects.AddRange(jobObjects);
        }

        public JobObject GetJobObject(string path)
        {
            return _jobObjects.FirstOrDefault(u => u.Path == path);
        }

        public void DeleteJobObject(string path)
        {
            JobObject jobObject = GetJobObject(path);
            if (jobObject is null)
                throw new BackupsException(path);

            _jobObjects.Remove(GetJobObject(path));
        }

        public RestorePoint CreateRestorePoint()
        {
            var restorePoint = new RestorePoint(_jobObjects, StorageType, _restorePoints.Count + 1);
            _restorePoints.Add(restorePoint);
            restorePoint.Create(DirectoryPath);
            return restorePoint;
        }
    }
}
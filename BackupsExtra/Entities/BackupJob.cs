using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Backups.Entities;
using Backups.Entities.Files;
using Backups.Enums;
using BackupsExtra.Algorithms.CleaningAlgorithms;
using BackupsExtra.Enums;
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

        [DataMember(Name = "CleaningAlgorithm")]
        private readonly AbstractCleaningAlgorithm _cleaningAlgorithm;

        [DataMember(Name = "LimitBehavior")]
        private readonly LimitBehavior _limitBehavior;

        internal BackupJob(
            string path,
            StorageTypes storageType,
            AbstractLogger logger,
            AbstractCleaningAlgorithm cleaningAlgorithm,
            LimitBehavior limitBehavior)
        {
            _backupJob = new Backups.Entities.BackupJob(path, storageType);
            _logger = logger;
            _cleaningAlgorithm = cleaningAlgorithm;
            _limitBehavior = limitBehavior;

            _logger?.Log("Created backup job");
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
                _logger?.Log($"Added job object: {jobObject.Path}");
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
                    _logger?.Log($"Deleted job object: {path}");
                }
            }
            catch (NullReferenceException e)
            {
                _logger?.Log($"Failed to delete job object {e.Message}");
                throw new BackupsExtraException(e.Message);
            }
        }

        public RestorePoint CreateRestorePoint()
        {
            return CreateRestorePoint(DateTime.Now);
        }

        public RestorePoint CreateRestorePoint(DateTime time)
        {
            try
            {
                List<AbstractJobObject> jobObjects = _backupJob.GetJobObjects();

                if (jobObjects.Count == 0)
                    throw new BackupsExtraException("No objects to backup");

                int number = RestorePointsCount == 0 ? 0 :
                    ((RestorePoint)_backupJob
                    .GetRestorePoints()
                    .OrderByDescending(u => ((RestorePoint)u).Number)
                    .First())
                    .Number;

                number++;

                var restorePoint = new RestorePoint(
                        new List<AbstractJobObject>(jobObjects),
                        StorageType,
                        number,
                        _logger);

                _backupJob.GetRestorePoints().Add(restorePoint);
                restorePoint.Create(DirectoryPath, time);
                if (_cleaningAlgorithm is not null)
                    ClearRestorePoints();
                _logger?.Log($"Created restore point number {number}");
                return restorePoint;
            }
            catch (BackupsExtraException e)
            {
                _logger?.Log($"Failed to create restore point: {e.Message}");
                throw new BackupsExtraException(e.Message);
            }
        }

        public RestorePoint GetRestorePoint(int number)
        {
            return (RestorePoint)_backupJob
                .GetRestorePoints()
                .First(u =>
                    ((RestorePoint)u).Number == number);
        }

        public void DeleteRestorePoint(int number)
        {
            RestorePoint restorePoint =
                GetRestorePoint(number)
                ?? throw new BackupsExtraException(
                    $"Can't find restore point number {number}");

            restorePoint.Delete();
            _backupJob
                .GetRestorePoints()
                .Remove(restorePoint);
        }

        private void ClearRestorePoints()
        {
            List<AbstractRestorePoint> restorePoints = _backupJob.GetRestorePoints();

            List<AbstractRestorePoint> remainingPoints =
                _cleaningAlgorithm
                    .GetValidPoints(restorePoints);

            if (remainingPoints.Count == 0)
            {
                const string errorMessage = "Can't delete all restore points";
                _logger?.Log(errorMessage);
                throw new BackupsExtraException(errorMessage);
            }

            var pointsToDelete = restorePoints
                .Except(remainingPoints)
                .ToList();

            pointsToDelete.ForEach(u =>
            {
                var restorePoint = (RestorePoint)u;
                try
                {
                    switch (_limitBehavior)
                    {
                        case LimitBehavior.Delete:
                            restorePoint.Delete();
                            break;
                        case LimitBehavior.Merge:
                            ((RestorePoint)remainingPoints
                                .First())
                                .Merge(restorePoint);
                            break;
                        default:
                            throw new BackupsExtraException(
                                _limitBehavior.ToString());
                    }

                    restorePoints.Remove(u);
                    _logger?.Log($"Restore point number {restorePoint.Number} cleared");
                }
                catch (BackupsExtraException e)
                {
                    _logger?.Log($"Failed to clearRestorePoints: {e.Message}");
                    throw new BackupsExtraException(e.Message);
                }
            });
        }
    }
}
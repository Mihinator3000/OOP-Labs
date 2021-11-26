using System.Collections.Generic;
using System.Runtime.Serialization;
using BackupsExtra.Algorithms.ConfigAlgorithms;
using BackupsExtra.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    [DataContract]
    public class BackupService
    {
        [DataMember(Name="BackupJobs")]
        private readonly List<BackupJob> _backupJobs = new ();

        public void AddBackupJob(BackupJob backupJob)
        {
            _backupJobs.Add(backupJob);
        }

        public BackupJob GetBackupJob(int id)
        {
            if (id < 0 || id >= _backupJobs.Count)
                throw new BackupsExtraException("Invalid id");

            return _backupJobs[id];
        }

        public void DeleteBackupJob(BackupJob backupJob)
        {
            if (!_backupJobs.Remove(backupJob))
                throw new BackupsExtraException("Backup job was not found");
        }

        public void Save()
        {
            new ConfigHandler()
                .Save(this);
        }

        public void Save(string path)
        {
            new ConfigHandler(path)
                .Save(this);
        }
    }
}
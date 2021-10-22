using System;
using System.Collections.Generic;
using Backups.Entities.Files;

namespace Backups.Entities
{
    public interface IRestorePoint
    {
        public DateTime CreationTime { get; }

        public List<IJobObject> JobObjects { get; }

        public void Create(string directoryPath);
    }
}
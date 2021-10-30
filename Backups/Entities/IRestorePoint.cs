using System;
using System.Collections.Generic;
using Backups.Entities.Files;

namespace Backups.Entities
{
    public interface IRestorePoint
    {
        DateTime CreationTime { get; }

        List<IJobObject> JobObjects { get; }

        void Create(string directoryPath);
    }
}
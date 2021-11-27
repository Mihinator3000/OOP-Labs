using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Backups.Entities.Files;

namespace Backups.Entities
{
    [DataContract, KnownType(typeof(RestorePoint))]
    public abstract class AbstractRestorePoint
    {
        [DataMember]
        public abstract DateTime CreationTime { get; protected set; }

        [DataMember]
        public abstract List<AbstractJobObject> JobObjects { get; protected set; }

        public abstract void Create(string directoryPath);
    }
}
using System.Runtime.Serialization;

namespace Backups.Entities.Files
{
    [DataContract, KnownType(typeof(JobObject))]
    public abstract class AbstractJobObject
    {
        [DataMember]
        public abstract string Path { get; protected set; }

        public abstract string Name { get; }

        public abstract string NameWithoutExtension { get; }

        public abstract bool Exists();
    }
}
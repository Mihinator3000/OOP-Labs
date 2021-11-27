using System.Runtime.Serialization;
using Backups.Tools;

namespace Backups.Entities.Files
{
    [DataContract]
    public class JobObject : AbstractJobObject
    {
        public JobObject(string path)
        {
            Path = path;
        }

        [DataMember]
        public sealed override string Path { get; protected set; }

        public override string Name =>
            System.IO.Path.GetFileName(Path);

        public override string NameWithoutExtension =>
            System.IO.Path.GetFileNameWithoutExtension(Name)
            ?? throw new BackupsException(Name);

        public override bool Exists() =>
            System.IO.File.Exists(Path);
    }
}
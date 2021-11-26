using System.IO;
using System.Runtime.Serialization;

namespace Backups.Entities.Files
{
    [DataContract]
    public class Storage : IStorage
    {
        [DataMember(Name = "Number")]
        private readonly int _number;

        public Storage(int number)
        {
            _number = number;
        }

        public Storage(string directoryPath, int number)
            : this(number)
        {
            if (directoryPath is not null)
                DirectoryPath = directoryPath;
        }

        [DataMember(Name = "DirectoryPath")]
        public string DirectoryPath { get; protected set; } = string.Empty;

        public string FullPath() =>
            Path.Combine(DirectoryPath, $"archive_{_number}.zip");

        public string FullPath(string filename) =>
            Path.Combine(DirectoryPath, $"{filename}_{_number}.zip");
    }
}

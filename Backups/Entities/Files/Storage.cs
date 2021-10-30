using System.IO;

namespace Backups.Entities.Files
{
    public class Storage : IStorage
    {
        private readonly string _directoryPath = string.Empty;
        private readonly int _number;

        public Storage(int number)
        {
            _number = number;
        }

        public Storage(string directoryPath, int number)
            : this(number)
        {
            if (directoryPath is not null)
                _directoryPath = directoryPath;
        }

        public string FullPath() =>
            Path.Combine(_directoryPath, $"archive_{_number}.zip");

        public string FullPath(string filename) =>
            Path.Combine(_directoryPath, $"{filename}_{_number}.zip");
    }
}

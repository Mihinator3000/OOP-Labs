using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Entities.Files;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SplitStorageAlgorithm : IStorageAlgorithm
    {
        public void Create(List<IJobObject> jobObjects, IStorage storage)
        {
            jobObjects.ForEach(u =>
            {
                string archivePath = storage.FullPath(u.NameWithoutExtension);

                if (File.Exists(archivePath))
                    throw new BackupsException(archivePath);

                ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);

                archive.CreateEntryFromFile(u.Path, u.Name, CompressionLevel.Optimal);
                archive.Dispose();
            });
        }
    }
}
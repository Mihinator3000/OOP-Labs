using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Entities.Files;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SplitStorageAlgorithm : IStorageAlgorithm
    {
        public void Create(List<AbstractJobObject> jobObjects, IStorage storage)
        {
            jobObjects.ForEach(jobObject =>
            {
                string archivePath = storage.FullPath(jobObject.NameWithoutExtension);

                if (File.Exists(archivePath))
                    throw new BackupsException($"Archive {archivePath} already exists");

                if (!jobObject.Exists())
                    throw new BackupsException($"File {jobObject.Path} does not exist");

                ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);

                archive.CreateEntryFromFile(jobObject.Path, jobObject.Name, CompressionLevel.Optimal);
                archive.Dispose();
            });
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Entities.Files;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SingleStorageAlgorithm : IStorageAlgorithm
    {
        public void Create(List<AbstractJobObject> jobObjects, IStorage storage)
        {
            string archivePath = storage.FullPath();

            if (File.Exists(archivePath))
                throw new BackupsException($"Archive {archivePath} already exists");

            jobObjects.ForEach(jobObject =>
            {
                if (!jobObject.Exists())
                    throw new BackupsException($"File {jobObject.Path} does not exist");
            });

            ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);
            foreach (AbstractJobObject jobObject in jobObjects)
            {
                archive.CreateEntryFromFile(jobObject.Path, jobObject.Name, CompressionLevel.Optimal);
            }

            archive.Dispose();
        }
    }
}
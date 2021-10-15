using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Entities.Files;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SingleStorageAlgorithm : IStorageAlgorithm
    {
        public void Create(List<JobObject> jobObjects, Storage storage)
        {
            string archivePath = storage.FullPath();

            if (File.Exists(archivePath))
                throw new BackupsException(archivePath);

            ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);
            foreach (JobObject jobObject in jobObjects)
            {
                if (!File.Exists(jobObject.Path))
                    throw new BackupsException($"{jobObject.Path} does not exist");

                archive.CreateEntryFromFile(jobObject.Path, jobObject.Name, CompressionLevel.Optimal);
            }

            archive.Dispose();
        }
    }
}
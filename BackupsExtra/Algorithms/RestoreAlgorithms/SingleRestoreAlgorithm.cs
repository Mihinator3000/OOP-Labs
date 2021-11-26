using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Entities.Files;
using BackupsExtra.Tools;

namespace BackupsExtra.Algorithms.RestoreAlgorithms
{
    public class SingleRestoreAlgorithm : IRestoreAlgorithm
    {
        public void Restore(List<AbstractJobObject> jobObjects, IStorage storage, string extractionPath)
        {
            string archivePath = storage.FullPath();

            if (!File.Exists(archivePath))
                throw new BackupsExtraException($"Archive does not exist");

            ZipFile.ExtractToDirectory(archivePath, extractionPath, true);
        }
    }
}
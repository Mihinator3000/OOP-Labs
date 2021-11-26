using System.Collections.Generic;
using Backups.Entities.Files;

namespace BackupsExtra.Algorithms.RestoreAlgorithms
{
    public interface IRestoreAlgorithm
    {
        void Restore(List<AbstractJobObject> jobObjects, IStorage storage, string extractionPath);
    }
}
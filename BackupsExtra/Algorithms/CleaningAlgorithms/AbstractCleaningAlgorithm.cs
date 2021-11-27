using System.Collections.Generic;
using System.Runtime.Serialization;
using Backups.Entities;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    [DataContract]
    [KnownType(typeof(TimeCleaningAlgorithm))]
    [KnownType(typeof(CountCleaningAlgorithm))]
    [KnownType(typeof(HybridCleaningAlgorithm))]
    public abstract class AbstractCleaningAlgorithm
    {
        public abstract List<AbstractRestorePoint> GetValidPoints(List<AbstractRestorePoint> restorePoints);
    }
}
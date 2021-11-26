using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    public interface ICleaningAlgorithm
    {
        List<RestorePoint> GetValidPoints(List<RestorePoint> restorePoints);
    }
}
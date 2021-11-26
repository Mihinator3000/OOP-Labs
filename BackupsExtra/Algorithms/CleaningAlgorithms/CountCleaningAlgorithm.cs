using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Tools;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    public class CountCleaningAlgorithm : ICleaningAlgorithm
    {
        private readonly int _count;

        public CountCleaningAlgorithm(int count)
        {
            if (count <= 0)
                throw new BackupsException("Number of points for cleaning algorithm is invalid");

            _count = count;
        }

        public List<RestorePoint> GetValidPoints(List<RestorePoint> restorePoints)
        {
            return restorePoints
                .TakeLast(_count)
                .ToList();
        }
    }
}
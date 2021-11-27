using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Backups.Entities;
using Backups.Tools;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    [DataContract]
    public class CountCleaningAlgorithm : AbstractCleaningAlgorithm
    {
        [DataMember(Name = "Count")]
        private readonly int _count;

        public CountCleaningAlgorithm(int count)
        {
            if (count <= 0)
                throw new BackupsException("Number of points for cleaning algorithm is invalid");

            _count = count;
        }

        public override List<AbstractRestorePoint> GetValidPoints(List<AbstractRestorePoint> restorePoints)
        {
            return restorePoints
                .TakeLast(_count)
                .ToList();
        }
    }
}
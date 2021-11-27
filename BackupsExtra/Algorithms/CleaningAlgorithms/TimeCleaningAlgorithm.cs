using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Backups.Entities;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    [DataContract]
    public class TimeCleaningAlgorithm : AbstractCleaningAlgorithm
    {
        [DataMember(Name = "Time")]
        private readonly TimeSpan _time;

        public TimeCleaningAlgorithm(TimeSpan time)
        {
            _time = time;
        }

        public override List<AbstractRestorePoint> GetValidPoints(List<AbstractRestorePoint> restorePoints)
        {
            DateTime mininalCreationTime = DateTime.Now - _time;

            return restorePoints
                .Where(u => u.CreationTime > mininalCreationTime)
                .ToList();
        }
    }
}
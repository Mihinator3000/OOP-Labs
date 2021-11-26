using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    public class DateCleaningAlgorithm : ICleaningAlgorithm
    {
        private readonly DateTime _dateTime;

        public DateCleaningAlgorithm(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public List<RestorePoint> GetValidPoints(List<RestorePoint> restorePoints)
        {
            return restorePoints
                .Where(u => u.CreationTime > _dateTime)
                .ToList();
        }
    }
}
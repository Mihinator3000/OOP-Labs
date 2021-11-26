using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.Enums;
using BackupsExtra.Tools;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    public class HybridCleaningAlgorithm : ICleaningAlgorithm
    {
        private readonly List<ICleaningAlgorithm> _cleaningAlgorithms;
        private readonly CleaningConditions _cleaningConditions;

        public HybridCleaningAlgorithm(
            CleaningConditions cleaningConditions,
            params ICleaningAlgorithm[] cleaningAlgorithms)
        {
            if (cleaningAlgorithms is null)
                throw new NullReferenceException(nameof(cleaningAlgorithms));

            if (cleaningAlgorithms.Length < 2)
                throw new BackupsExtraException("Incorrect number of algorithms");

            _cleaningAlgorithms = cleaningAlgorithms.ToList();

            _cleaningConditions = cleaningConditions;
        }

        public List<RestorePoint> GetValidPoints(List<RestorePoint> restorePoints)
        {
            List<RestorePoint> validPoints = restorePoints;

            foreach (ICleaningAlgorithm cleaningAlgorithm in _cleaningAlgorithms)
            {
                if (validPoints.Count == 0)
                    break;

                List<RestorePoint> algorithmValidPoints = cleaningAlgorithm.GetValidPoints(restorePoints);
                validPoints = _cleaningConditions switch
                {
                    CleaningConditions.DoesNotFitOneLimit =>
                        validPoints
                            .Intersect(algorithmValidPoints)
                            .ToList(),
                    CleaningConditions.DoesNotFitAllLimits =>
                        validPoints
                            .Concat(algorithmValidPoints)
                            .Distinct()
                            .ToList(),
                    _ => throw new ArgumentOutOfRangeException(nameof(_cleaningConditions)),
                };
            }

            return validPoints;
        }
    }
}
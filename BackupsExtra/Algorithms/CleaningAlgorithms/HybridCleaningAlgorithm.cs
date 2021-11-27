using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Backups.Entities;
using BackupsExtra.Enums;
using BackupsExtra.Tools;

namespace BackupsExtra.Algorithms.CleaningAlgorithms
{
    [DataContract]
    public class HybridCleaningAlgorithm : AbstractCleaningAlgorithm
    {
        [DataMember(Name = "CleaningAlgorithms")]
        private readonly List<AbstractCleaningAlgorithm> _cleaningAlgorithms;

        [DataMember(Name = "CleaningConditions")]
        private readonly CleaningConditions _cleaningConditions;

        public HybridCleaningAlgorithm(
            CleaningConditions cleaningConditions,
            params AbstractCleaningAlgorithm[] cleaningAlgorithms)
        {
            if (cleaningAlgorithms is null)
                throw new NullReferenceException(nameof(cleaningAlgorithms));

            if (cleaningAlgorithms.Length < 2)
                throw new BackupsExtraException("Incorrect number of algorithms");

            _cleaningAlgorithms = cleaningAlgorithms.ToList();

            _cleaningConditions = cleaningConditions;
        }

        public override List<AbstractRestorePoint> GetValidPoints(List<AbstractRestorePoint> restorePoints)
        {
            List<AbstractRestorePoint> validPoints = _cleaningAlgorithms[0]
                    .GetValidPoints(restorePoints);

            foreach (AbstractCleaningAlgorithm cleaningAlgorithm in _cleaningAlgorithms)
            {
                if (validPoints.Count == 0)
                    break;

                List<AbstractRestorePoint> algorithmValidPoints = cleaningAlgorithm.GetValidPoints(restorePoints);

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
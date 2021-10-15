using System.Collections.Generic;
using Backups.Entities.Files;

namespace Backups.Algorithms
{
    public interface IStorageAlgorithm
    {
        void Create(List<JobObject> jobObjects, Storage storage);
    }
}
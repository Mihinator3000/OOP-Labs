using System.Collections.Generic;
using System.Linq;
using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;

namespace Reports.DataAccessLayer.Entities
{
    public class DbTask
    {
        public int Id { get; set; }

        public TaskStates State { get; set; }

        public List<DbChange> Changes { get; set; }

        public static DbTask FromDto(TaskDto task)
        {
            return new DbTask
            {
                Id = task.Id,
                State = task.State,
                Changes = task.Changes.Select(DbChange.FromDto).ToList(),
            };
        }

        public TaskDto ToDto()
        {
            return new TaskDto
            {
                Id = Id,
                State = State,
                Changes = Changes.Select(u => u.ToDto()).ToList()
            };
        }
    }
}
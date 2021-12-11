using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;

namespace Reports.DataAccessLayer.Entities
{
    public class DbTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public TaskStates State { get; set; }

        public DateTime CreationTime { get; set; }

        public List<DbChange> Changes { get; set; } = new ();

        public DbUser AssignedUser { get; set; }

        public List<DbComment> Comments { get; set; } = new ();

        public static DbTask FromDto(TaskDto task)
        {
            var dbTask = new DbTask();
            dbTask.Update(task);
            return dbTask;
        }

        public void Update(TaskDto task)
        {
            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            State = task.State;
        }

        public TaskDto ToDto()
        {
            return new TaskDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                State = State,
                CreationTime = CreationTime,
                AssignedUser = AssignedUser?.ToDto(),
                Comments = Comments?.Select(u => u.ToDto()).ToList(),
                Changes = Changes?.Select(u => u.ToDto()).ToList()
            };
        }
    }
}
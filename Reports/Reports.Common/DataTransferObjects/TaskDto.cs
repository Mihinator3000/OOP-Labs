using System;
using System.Collections.Generic;
using Reports.Common.Enums;

namespace Reports.Common.DataTransferObjects
{
    public class TaskDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TaskStates State { get; set; }

        public DateTime CreationTime { get; set; }

        public List<ChangeDto> Changes { get; set; }

        public List<CommentDto> Comments { get; set; }

        public UserDto AssignedUser { get; set; }
    }
}
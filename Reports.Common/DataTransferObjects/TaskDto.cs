using System.Collections.Generic;
using Reports.Common.Enums;

namespace Reports.Common.DataTransferObjects
{
    public class TaskDto
    {
        public int Id { get; init; }

        public TaskStates State { get; init; }

        public List<ChangeDto> Changes { get; init; }
    }
}
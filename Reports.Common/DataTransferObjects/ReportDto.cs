using System.Collections.Generic;
using Reports.Common.Enums;

namespace Reports.Common.DataTransferObjects
{
    public class ReportDto
    {
        public int Id { get; set; }

        public UserDto User { get; set; }

        public string Message { get; set; }

        public ReportStates State { get; set; }

        public List<TaskDto> Tasks { get; set; }
    }
}
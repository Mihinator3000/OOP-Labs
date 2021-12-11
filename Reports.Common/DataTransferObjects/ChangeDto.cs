using System;
using Reports.Common.Enums;

namespace Reports.Common.DataTransferObjects
{
    public class ChangeDto
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public TaskChangeTypes ChangeType { get; set; }
        
        public string Message { get; set; }

        public UserDto User { get; set; }
    }
}
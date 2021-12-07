using System;
using Reports.Common.Enums;

namespace Reports.Common.DataTransferObjects
{
    public class ChangeDto
    {
        public int Id { get; init; }

        public DateTime Time { get; init; }

        public TaskChangeTypes ChangeType { get; init; }
        
        public string Message { get; init; }
    }
}
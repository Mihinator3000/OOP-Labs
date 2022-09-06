using System;
using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;

namespace Reports.DataAccessLayer.Entities
{
    public class DbChange
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public TaskChangeTypes ChangeType { get; set; }

        public string Message { get; set; }

        public DbUser User { get; set; }

        public static DbChange FromDto(ChangeDto change)
        {
            return new DbChange
            {
                Id = change.Id,
                Time = change.Time,
                ChangeType = change.ChangeType,
                Message = change.Message,
                User = DbUser.FromDto(change.User)
            };
        }

        public ChangeDto ToDto()
        {
            return new ChangeDto
            {
                Id = Id,
                Time = Time,
                ChangeType = ChangeType,
                Message = Message,
                User = User.ToDto()
            };
        }
    }
}
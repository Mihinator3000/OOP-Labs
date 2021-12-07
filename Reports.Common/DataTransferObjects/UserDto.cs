using Reports.Common.Enums;

namespace Reports.Common.DataTransferObjects
{
    public class UserDto
    {
        public int Id { get; set; }

        public UserTypes UserType { get; init; }

        public int? LeaderId { get; set; }
    }
}
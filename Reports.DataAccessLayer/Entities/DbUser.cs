using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;

namespace Reports.DataAccessLayer.Entities
{
    public class DbUser
    {
        public int Id { get; set; }

        public UserTypes UserType { get; set; }

        public int? LeaderId { get; set; }

        public static DbUser FromDto(UserDto user)
        {
            var dbUser = new DbUser();
            dbUser.Update(user);
            return dbUser;
        }

        public void Update(UserDto user)
        {
            Id = user.Id;
            UserType = user.UserType;
            LeaderId = user.LeaderId;
        }

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = Id,
                UserType = UserType,
                LeaderId = LeaderId
            };
        }
    }
}
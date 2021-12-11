using Reports.Common.DataTransferObjects;

namespace Reports.DataAccessLayer.Entities
{
    public class DbUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

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
            Name = user.Name;
            LeaderId = user.LeaderId;
        }

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = Id,
                Name = Name,
                LeaderId = LeaderId
            };
        }
    }
}
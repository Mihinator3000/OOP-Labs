using Reports.Common.DataTransferObjects;

namespace Reports.DataAccessLayer.Entities
{
    public class DbLogin
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int UserId { get; set; }

        public static DbLogin FromDto(LoginDto info)
        {
            return new DbLogin
            {
                Id = info.Id,
                Login = info.Login,
                Password = info.Password,
                UserId = info.UserId
            };
        }

        public LoginDto ToDto()
        {
            return new LoginDto
            {
                Id = Id,
                Login = Login,
                Password = Password,
                UserId = UserId
            };
        }
    }
}
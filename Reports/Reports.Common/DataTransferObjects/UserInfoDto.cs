using System.Collections.Generic;
using System.Text.Json.Serialization;
using Reports.Common.Enums;

namespace Reports.Common.DataTransferObjects
{
    public class UserInfoDto
    {
        public UserDto User { get; set; }

        [JsonIgnore]
        public int Id
        {
            get => User.Id;
            set => User.Id = value;
        }

        [JsonIgnore]
        public string Name
        {
            get => User.Name;
            set => User.Name = value;
        }

        [JsonIgnore]
        public int? LeaderId
        {
            get => User.LeaderId;
            set => User.LeaderId = value;
        }

        public UserDto Leader { get; set; }
        
        public UserTypes UserType { get; set; }
        

        public List<UserDto> Subordinates { get; set; }
    }
}
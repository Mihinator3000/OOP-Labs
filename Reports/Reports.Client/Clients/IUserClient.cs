using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Reports.Common.DataTransferObjects;

namespace Reports.Client.Clients
{
    public interface IUserClient
    {
        [Get("/api/user/get/all")]
        Task<List<UserDto>> GetAll();

        [Get("/api/user/get/{id}")]
        Task<UserInfoDto> GetById(int id);

        [Post("/api/user/create")]
        Task Create([Body] UserDto user);

        [Post("/api/user/delete/{id}")]
        Task Delete(int id);

        [Post("/api/user/update")]
        Task Update([Body] UserDto user);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Common.DataTransferObjects;

namespace Reports.DataAccessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();

        Task<UserDto> GetById(int id);

        Task Create(UserDto user);

        Task Delete(int id);

        Task Update(UserDto user);
    }
}
using System.Threading.Tasks;
using Reports.Common.DataTransferObjects;

namespace Reports.DataAccessLayer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> GetId(LoginDto info);

        Task CreateAccount(LoginDto info);

        Task DeleteAccount(int userId);
    }
}
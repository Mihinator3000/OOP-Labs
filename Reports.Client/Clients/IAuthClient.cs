using System.Threading.Tasks;
using Refit;
using Reports.Common.DataTransferObjects;

namespace Reports.Client.Clients
{
    public interface IAuthClient
    {
        [Get("/auth/getId")]
        Task<int> GetId([Body] LoginDto info);

        [Post("/auth/create")]
        Task CreateAccount([Body] LoginDto info);

        [Post("/auth/delete/{userId}")]
        Task DeleteAccount(int userId);
    }
}
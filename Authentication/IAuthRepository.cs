using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Authentication
{
    public interface IAuthRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}

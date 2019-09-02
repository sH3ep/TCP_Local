using System.Threading.Tasks;
using TPC.Api.Model;

namespace TPC.Api.Shared
{
    public interface ICurrentUserService
    {
        long GetCurrentUserId();
        Task<User> GetCurrentUser();
    }
}

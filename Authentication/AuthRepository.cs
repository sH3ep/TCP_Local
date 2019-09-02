using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Authentication
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        public AuthRepository(TcpContext context) : base(context)
        {

        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return Entities.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}

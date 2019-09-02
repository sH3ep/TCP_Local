using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Model.Lookups;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Users
{
    public class UserRepository :Repository<User>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Role> _roles;
        private readonly DbSet<Project> _projects;
        public UserRepository(TcpContext context, IUnitOfWork unitOfWork) : base(context)
        {

            _unitOfWork = unitOfWork;
            _roles = context.Set<Role>();
            _projects = context.Set<Project>();
        }

        public async Task<IEnumerable<Project>> GetAllOwnedProjects(long userId)
        {
            var projects = await _projects.Where(x => x.ModifiedBy == userId).ToListAsync();
            return projects;
        }

        public async Task<User> GetByEmail(string userEmail)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));
        }

        public async Task<bool> SetDeleted(long userId)
        {
            var user = await Get(userId);
            user.Email = null;
            user.PasswordHash = new byte[] {};
            user.PasswordSalt = new byte[] {};
            user.Activated = false;
            Update(user);
            return true;
        }
    }
}

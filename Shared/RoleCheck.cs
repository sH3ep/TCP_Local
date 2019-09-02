using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model.Lookups;
using TPC.Api.Model.ManyToManyRelations;
using TPC.Api.Persistence;

namespace TPC.Api.Shared
{
    public class RoleCheck : IRoleCheck
    {
        private readonly DbSet<Role> _roles;
        private readonly DbSet<UserProject> _userProjects;
        protected TcpContext Context { get; }

        public RoleCheck(TcpContext context)
        {
            Context = context;
            context.Database.EnsureCreated();
            _roles = Context.Set<Role>();
            _userProjects = Context.Set<UserProject>();
        }

        public async Task<bool> IsAdmin(long userId, long projectId)
        {
            var adminRole = await _roles.SingleAsync(x => x.Name == "Administrator");

            var userProject =
                await _userProjects.FirstOrDefaultAsync(x => x.UserId == userId && x.ProjectId == projectId);

            return userProject.RoleId == adminRole.Id;
        }

        public async Task<int> CountAdmins(IEnumerable<UserProject> userProjects)
        {
            var adminRole = await _roles.SingleAsync(x => x.Name == "Administrator");
            var adminUserProjects = userProjects.Where(x => x.RoleId == adminRole.Id);
            return adminUserProjects.Count();
        }
    }
}
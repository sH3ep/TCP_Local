using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model.ManyToManyRelations;

namespace TPC.Api.Shared
{
    public interface IRoleCheck
    {
        Task<bool> IsAdmin(long userId, long projectId);
        Task<int> CountAdmins(IEnumerable<UserProject> userProjects);
    }
}
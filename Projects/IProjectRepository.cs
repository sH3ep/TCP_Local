using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Model.ManyToManyRelations;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Projects
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task Activate(long projectId, long userId);

        Task<Project> GetActiveByUserId(long userId);

        Task<IEnumerable<Project>> GetAllByUserId(long userId);

        Task<IEnumerable<User>> GetProjectUsers(long projectId);

        Task<IEnumerable<Sprint>> GetProjectSprints(long projectId);

        Task<UserProject> AddUserProject(long projectId, long userId);

        Task<UserProject> AddUserProject(UserProject userProject);

        void DeleteUserFromProject(long projectId, long userId);

        void EditUserRole(long projectId, long userId, long roleId);

        Task DeleteProject(long projectId, long userId);

        Task<UserProject> GetUserRole(long projectId, long userId);

        Task<bool> HasManyAdmins(long projectId);

        Task<bool> SetUnassignedByUserId(long userId);
        Task<IEnumerable<UserProject>> GetUserProjectByProjectId(long projectId);


    }
}
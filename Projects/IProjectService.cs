using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model;
using TPC.Api.Model.ManyToManyRelations;
using TPC.Api.Shared;

namespace TPC.Api.Projects
{
    public interface IProjectService
    {
        Task<Project> Add(Project item, long userId);
        Task<UserProject> AddUserProject(long projectId, long userId);
        Task<Project> CreateNewProject(long userId, Project project);
        Task<IEnumerable<Project>> GetAllByUserId(long userId);
        Task DeleteProject(long projectId, long userId);
        Task<long> GetUserRole(long projectId,long userId);
        Task<ActionEffect> DeleteUserFromProject(long projectId, long userIdToDelete, long loggedUserId);
        Task<ActionEffect> EditUserRole(long projectId, long userId, long roleId, long loggedUserId);
        Task<ActionEffect> CanLeaveProject(long userId, long projectId);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Tasks
{
    public interface ITaskRepository:IRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetAllByUserId(long userId);
        Task<IEnumerable<TaskItem>> GetAllByUserStoryId(long userStoryId);
        Task<IEnumerable<TaskItem>> GetAllByProjectId(long projectId);
        Task<bool> SetUnassignedByUserId(long userId);
        Task<bool> SetUnassignedByUserId(long userId,long userStoryId);
    }
}
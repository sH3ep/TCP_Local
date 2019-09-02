using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Sprints
{
    public interface ISprintRepository : IRepository<Sprint>
    {
        Task<IEnumerable<TaskItem>> GetSprintTasks(long sprintId);

        Task RemoveAllByProjectId(long projectId);

        Task<bool> SetUnassignedByUserId(long userId);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Tasks.Dto;

namespace TPC.Api.Tasks
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetFiltered(TaskFilterDto filter);
    }
}
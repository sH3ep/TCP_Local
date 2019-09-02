using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;
using TPC.Api.Projects;
using TPC.Api.Tasks;

namespace TPC.Api.Sprints
{
    public class SprintRepository : Repository<Sprint>, ISprintRepository
    {
        private readonly DbSet<TaskItem> _tasks;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public SprintRepository(
            TcpContext context, 
            ITaskRepository taskRepository, 
            IProjectRepository projectRepository) : base(context)
        {
            _tasks = context.Set<TaskItem>();
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        public override Sprint Add(Sprint entity)
        {
            var sprints = _projectRepository.GetProjectSprints(entity.ProjectId).Result;

            var overlap = false;

            foreach (var sprint in sprints)
            {
                if (entity.EndDate >= sprint.StartDate && entity.StartDate <= sprint.EndDate)
                {
                    overlap = true;
                }
            }

            if (overlap)
            {
                throw new InvalidOperationException("Sprints cannot overlap");
            }

            return base.Add(entity);
        }

        public async Task<IEnumerable<TaskItem>> GetSprintTasks(long sprintId)
        {
            return await _tasks.Where(x => x.SprintId == sprintId).ToListAsync();
        }

        public async Task RemoveAllByProjectId(long projectId)
        {
            var sprints = await Entities.Where(s => s.ProjectId == projectId).ToListAsync();
            foreach (var sprint in sprints)
            {
                Entities.Remove(sprint);
            }
        }

        public async Task<bool> SetUnassignedByUserId(long userId)
        {
            await Task.Yield();
            var sprints = Entities.Where(x => x.ModifiedBy == userId);

            foreach (var item in sprints)
            {
                item.ModifiedBy = 0;
            }
            
            return true;
        }

        public override async void Remove(Sprint entity)
        {
            var tasksToDelete = await GetSprintTasks(entity.Id);
            foreach (var taskItem in tasksToDelete)
            {
                taskItem.SprintId = 0;
            }
            base.Remove(entity);
        }
    }
}

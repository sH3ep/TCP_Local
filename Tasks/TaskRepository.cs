using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Tasks
{
    public class TaskRepository:Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(TcpContext context) : base(context) {}

        public async Task<IEnumerable<TaskItem>> GetAllByUserId(long userId)
        {
            var taskItems = await Entities.Where(x => x.AssignedUserId == userId).ToListAsync();
            return taskItems;
        }

        public async Task<IEnumerable<TaskItem>> GetAllByUserStoryId(long userStoryId)
        {
            var taskItems = await Entities.Where(x => x.UserStoryId == userStoryId).ToListAsync();
            return taskItems;
        }
        
        public async Task<IEnumerable<TaskItem>> GetAllByProjectId(long projectId)
        {
            var features = await Context.Set<Feature>().Where(f => f.ProjectId == projectId).ToListAsync();
            var userStories = await Context.Set<UserStory>().Where(u => features.Any(f=> f.Id == u.FeatureId)).ToListAsync();
            var taskItems = await Context.Set<TaskItem>().Where(t => userStories.Any(u=> u.Id == t.UserStoryId)).ToListAsync();
            return taskItems;
        }

        public async Task<bool> SetUnassignedByUserId(long userId)
        {
            await Task.Yield();
            var tasks = Entities.Where(x => x.AssignedUserId == userId).ToList();
            foreach (var item in tasks)
            {
                item.AssignedUserId = 0;
            }
            
            return true;
        }

        public async Task<bool> SetUnassignedByUserId(long userId,long userStoryId)
        {
            await Task.Yield();
            var tasks = Entities.Where(x => x.AssignedUserId == userId && x.UserStoryId == userStoryId).ToList();
            foreach (var item in tasks)
            {
                item.AssignedUserId = 0;
            }

            return true;
        }
    }
}
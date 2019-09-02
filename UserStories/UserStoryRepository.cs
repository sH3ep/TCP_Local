using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;
using TPC.Api.Tasks;

namespace TPC.Api.UserStories
{
    public class UserStoryRepository : Repository<UserStory>, IUserStoryRepository
    {
        private readonly ITaskRepository _taskRepository;

        public UserStoryRepository(TcpContext context, ITaskRepository taskRepository) : base(context)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<UserStory>> GetAllByUserId(long userId)
        {
            var userStories = await Entities.Where(x => x.AssignedUserId == userId).ToListAsync();
            return userStories;
        }

        public async Task<IEnumerable<UserStory>> GetAllByFeatureId(long featureId)
        {
            var userStories = await Entities.Where(x => x.FeatureId == featureId).ToListAsync();
            return userStories;
        }

        public async Task<bool> SetUnassignedByUserId(long userId)
        {
            await Task.Yield();
            var userStories = Entities.Where(x => x.AssignedUserId == userId).ToList();
            foreach (var item in userStories)
            {
                item.AssignedUserId = 0;
            }

            return true;
        }

        public async Task<bool> SetUnassignedByUserId(long userId, long featureId)
        {
            await Task.Yield();
            var userStories = Entities.Where(x => x.AssignedUserId == userId && x.FeatureId == featureId).ToList();
            foreach (var item in userStories)
            {
                await _taskRepository.SetUnassignedByUserId(userId, item.Id);
                item.AssignedUserId = 0;
            }

            return true;
        }

        public async Task<IEnumerable<UserStory>> GetAllByProjectId(long projectId)
        {
            var features = await Context.Set<Feature>().Where(f => f.ProjectId == projectId).ToListAsync();
            var userStories = await Context.Set<UserStory>().Where(u => features.Any(f=> f.Id == u.FeatureId)).ToListAsync();
            return userStories;
        }
    }
}

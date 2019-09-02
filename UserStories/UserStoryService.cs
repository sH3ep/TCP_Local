using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.UserStories.Dto;

namespace TPC.Api.UserStories
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IUserStoryRepository _userStoryRepository;

        // TODO repos instead of dbsets
        private readonly DbSet<Feature> _features;
        private readonly DbSet<UserStory> _userStories;
        private readonly DbSet<TaskItem> _tasks;

        public UserStoryService(IUserStoryRepository userStoryRepository, TcpContext context)
        {
            _userStoryRepository = userStoryRepository;

            _features = context.Set<Feature>();
            _userStories = context.Set<UserStory>();
            _tasks = context.Set<TaskItem>();
        }

        public async Task<IEnumerable<UserStory>> GetFiltered(UserStoryFilterDto filter)
        {
            List<UserStory> filteredUserStoryItems;

            if (filter.FeatureId > 0)
            {
                filteredUserStoryItems = (await _userStoryRepository.GetAllByFeatureId(filter.FeatureId)).ToList();
            }
            else
            {
                filteredUserStoryItems = (await _userStoryRepository.GetAll()).ToList();
            }

            if (filter.AssignedUsersId != null && filter.AssignedUsersId.Any())
            {
                filteredUserStoryItems = filteredUserStoryItems.Where(x => filter.AssignedUsersId.Contains(x.AssignedUserId))
                    .ToList();
            }

            if (filter.StatusesId != null && filter.StatusesId.Any())
            {
                filteredUserStoryItems = filteredUserStoryItems.Where(x => filter.StatusesId.Contains(x.StatusId))
                    .ToList();
            }

            return filteredUserStoryItems;
        }
    }
}

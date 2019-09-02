using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Features.Dto;

namespace TPC.Api.Features
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;

        // TODO repos instead of dbsets
        private readonly DbSet<Feature> _features;
        private readonly DbSet<UserStory> _userStories;
        private readonly DbSet<TaskItem> _tasks;

        public FeatureService(IFeatureRepository featureRepository, TcpContext context)
        {
            _featureRepository = featureRepository;

            _features = context.Set<Feature>();
            _userStories = context.Set<UserStory>();
            _tasks = context.Set<TaskItem>();
        }

        public async Task<IEnumerable<Feature>> GetFiltered(FeatureFilterDto filter)
        {
            List<Feature> filteredFeatureItems;

            if (filter.ProjectId > 0)
            {
                filteredFeatureItems = (await _featureRepository.GetAllByProjectId(filter.ProjectId)).ToList();
            }
            else
            {
                filteredFeatureItems = (await _featureRepository.GetAll()).ToList();
            }

            if (filter.AssignedUsersId != null && filter.AssignedUsersId.Any())
            {
                filteredFeatureItems = filteredFeatureItems.Where(x => filter.AssignedUsersId.Contains(x.AssignedUserId))
                    .ToList();
            }

            if (filter.StatusesId != null && filter.StatusesId.Any())
            {
                filteredFeatureItems = filteredFeatureItems.Where(x => filter.StatusesId.Contains(x.StatusId))
                    .ToList();
            }

            return filteredFeatureItems;
        }
    }
}

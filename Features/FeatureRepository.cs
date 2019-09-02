using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;
using TPC.Api.UserStories;

namespace TPC.Api.Features
{
    public class FeatureRepository : Repository<Feature>, IFeatureRepository
    {
        private readonly IUserStoryRepository _userStoryRepository;

        public FeatureRepository(TcpContext context, IUserStoryRepository userStoryRepository) : base(context)
        {
            _userStoryRepository = userStoryRepository;
        }

        public async Task<IEnumerable<Feature>> GetAllByUserId(long userId)
        {
            var features = await Entities.Where(x => x.AssignedUserId == userId).ToListAsync();
            return features;
        }

        public async Task<IEnumerable<Feature>> GetAllByProjectId(long projectId)
        {
            var features = await Entities.Where(x => x.ProjectId == projectId).ToListAsync();
            return features;
        }

        public async Task<bool> SetUnassignedByUserId(long userId)
        {
            await Task.Yield();
            var features = Entities.Where(x => x.AssignedUserId == userId).ToList();
            foreach (var item in features)
            {
                item.AssignedUserId = 0;
            }

            return true;
        }

        public async Task<bool> SetUnassignedByUserId(long userId, long projectId)
        {
            await Task.Yield();
            var features = Entities.Where(x => x.AssignedUserId == userId && x.ProjectId == projectId).ToList();
            foreach (var item in features)
            {
                await _userStoryRepository.SetUnassignedByUserId(userId, item.Id);
                item.AssignedUserId = 0;
            }

            return true;
        }
    }
}

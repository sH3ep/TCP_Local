using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.UserStories
{
    public interface IUserStoryRepository : IRepository<UserStory>
    {
        Task<IEnumerable<UserStory>> GetAllByUserId(long userId);
        Task<IEnumerable<UserStory>> GetAllByFeatureId(long featureId);
        Task<bool> SetUnassignedByUserId(long userId);
        Task <bool> SetUnassignedByUserId(long userId,long featureId);
        Task<IEnumerable<UserStory>> GetAllByProjectId(long projectId);
    }
}

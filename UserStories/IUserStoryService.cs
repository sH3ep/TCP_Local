using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.UserStories.Dto;

namespace TPC.Api.UserStories
{
    public interface IUserStoryService
    {
        Task<IEnumerable<UserStory>> GetFiltered(UserStoryFilterDto filter);
    }
}

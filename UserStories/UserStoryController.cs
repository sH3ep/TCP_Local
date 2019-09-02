using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Shared;
using TPC.Api.UserStories.Dto;


namespace TPC.Api.UserStories
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserStoryController : CrudController<UserStory, IUserStoryRepository>
    {
        private readonly IUserStoryService _userStoryService;

        public UserStoryController(
            IUnitOfWork unitOfWork,
            IUserStoryRepository userStoryRepository,
            IMapper mapper,
            IUserStoryService userStoryService,
            IModificationService modificationService) :
                base(unitOfWork, userStoryRepository, mapper, modificationService)
        {
            _userStoryService = userStoryService;
        }

        [HttpGet("feature/{featureId}")]
        public async Task<IEnumerable<UserStory>> GetAllByFeatureId([FromRoute] long featureId)
        {
            return await Repository.GetAllByFeatureId(featureId);
        }
        
        [HttpGet("project/{projectId}")]
        public async Task<IEnumerable<UserStory>> GetAllByProjectId([FromRoute] long projectId)
        {
            return await Repository.GetAllByProjectId(projectId);
        }

        [HttpGet("filtered")]
        public async Task<IEnumerable<UserStory>> GetFilteredTasks([FromQuery]UserStoryFilterDto userStoryFilterDto)
        {
            var userStories = await _userStoryService.GetFiltered(userStoryFilterDto);
            return userStories;
        }
    }
}

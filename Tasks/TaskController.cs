using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Shared;
using TPC.Api.Tasks.Dto;

namespace TPC.Api.Tasks
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : CrudController<TaskItem, ITaskRepository>
    {
        private readonly ITaskService _taskService;

        public TaskController(
            IUnitOfWork unitOfWork,
            ITaskRepository repository,
            IMapper mapper,
            ITaskService taskService,
            IModificationService modificationService,
            ICurrentUserService currentUserService) : base(unitOfWork, repository, mapper, modificationService)
        {
            _taskService = taskService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<TaskItem>> GetAllByUserId([FromRoute] long userId)
        {
            return await Repository.GetAllByUserId(userId);
        }

        [HttpGet("userStory/{userStoryId}")]
        public async Task<IEnumerable<TaskItem>> GetAllByUserStoryId([FromRoute] long userStoryId)
        {
            return await Repository.GetAllByUserStoryId(userStoryId);
        }
        
        [HttpGet("project/{projectId}")]
        public async Task<IEnumerable<TaskItem>> GetAllByProjectId([FromRoute] long projectId)
        {
            return await Repository.GetAllByProjectId(projectId);
        }

        [HttpGet("filtered")]
        public async Task<IEnumerable<TaskItem>> GetFilteredTasks([FromQuery]TaskFilterDto taskFilterDto)
        {
            var tasks = await _taskService.GetFiltered(taskFilterDto);
            return tasks;
        }
    }
}
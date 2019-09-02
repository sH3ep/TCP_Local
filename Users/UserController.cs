using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model;
using TPC.Api.Model.Dto;
using TPC.Api.Persistence;
using TPC.Api.Projects;
using TPC.Api.Shared;

namespace TPC.Api.Users
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectService _projectService;
        private readonly IRoleCheck _roleCheck;
        private readonly IProjectRepository _projectRepository;

        public UserController(
            IUnitOfWork unitOfWork,
            IUserService userService,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IProjectService projectService,
            IRoleCheck roleCheck,
            IProjectRepository projectRepository)
        {
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _projectService = projectService;
            _roleCheck = roleCheck;
            _projectRepository = projectRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> Get()
        {
            long userId = _currentUserService.GetCurrentUserId();
            var user = await _userService.Get(userId);
            if (user == null)
            {
                return NotFound();
            }
            return _mapper.Map<UserDto>(user);
        }

        [HttpPut("OwnNameAndLastName")]
        public async Task<ActionResult<UserDto>> EditOwnNameAndLastName([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            long userId = _currentUserService.GetCurrentUserId();
            var user = await _userService.EditNameAndLastName(userId, userDto);
            await _unitOfWork.Complete();
            if (user != null)
            {
                return Ok(_mapper.Map<UserDto>(user));
            }

            return BadRequest(null);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOwnAccount()
        {
            var deleteEffect = await  _userService.DeleteOwnAccount(_currentUserService.GetCurrentUserId());
            if (deleteEffect.IsSuccesfull)
            {
                return Ok();
            }
            else
            {
                return BadRequest(deleteEffect.ErrorMessage);
            }
          
        }

        [HttpGet("CanBeSafeDeleted")]
        public async Task<ActionResult<IEnumerable<Project>>> CanBeSafeDeleted()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var userProjects = await _projectService.GetAllByUserId(userId);
            var projectToDelete = new List<Project>();
            foreach (var item in userProjects)
            {
                if (await _roleCheck.IsAdmin(userId, item.Id))
                {
                    if (await _roleCheck.CountAdmins(await _projectRepository.GetUserProjectByProjectId(item.Id)) < 2)
                    {
                        projectToDelete.Add(item);
                    }
                }
            }
            return projectToDelete.Any() ? Ok(projectToDelete) : null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TPC.Api.Authentication;
using TPC.Api.Authentication.Dto;
using TPC.Api.Model;
using TPC.Api.Model.Dto;
using TPC.Api.Persistence;
using TPC.Api.Projects.Dto;
using TPC.Api.Shared;

namespace TPC.Api.Projects
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : CrudController<Project, IProjectRepository>
    {
        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly AppSettings _appSettings;
        private readonly IAuthRepository _authRepository;
        private readonly IRoleCheck _roleCheck;
        private readonly IProjectRepository _projectRepository;

        public ProjectController(
            IUnitOfWork unitOfWork,
            IProjectService projectService,
            IAuthRepository authRepository,
            IProjectRepository projectRepository,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IAuthService authService,
            IRoleCheck roleCheck,
            IModificationService modificationService,
            ICurrentUserService currentUserService) :
            base(unitOfWork, projectRepository, mapper, modificationService)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _appSettings = appSettings.Value;
            _authRepository = authRepository;
            _roleCheck = roleCheck;
            _projectRepository = projectRepository;
        }

        [HttpPost]
        public override async Task<ActionResult<Project>> Post([FromBody] Project item)
        {
            if (!ModelState.IsValid)
                return BadRequest("Blad walidacji");

            var userId = _currentUserService.GetCurrentUserId();
            ModificationService.UpdateModifiedByModifiedDate(item);
            var project = await _projectService.CreateNewProject(userId, item);
            return Ok(project);
        }

        [HttpGet("active")]
        public async Task<ActionResult<Project>> GetActive()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var result = await Repository.GetActiveByUserId(userId);
            return Ok(result);
        }

        [HttpPost("activate/{projectId}")]
        public async Task<ActionResult> Activate([FromRoute]long projectId)
        {
            var userId = _currentUserService.GetCurrentUserId();
            await Repository.Activate(projectId, userId);
            await UnitOfWork.Complete();

            return Ok();
        }

        [HttpGet("view")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectDtos()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var result = await Repository.GetAllByUserId(userId);
            var projectDtos = Mapper.Map<IEnumerable<ProjectDto>>(result);

            var activeProject = await Repository.GetActiveByUserId(userId);

            if (activeProject != null)
            {
                foreach (var projectDto in projectDtos)
                {
                    if (activeProject.Id == projectDto.Id)
                    {
                        projectDto.Active = true;
                    }
                }
            }

            return Ok(projectDtos);
        }

        [HttpGet]
        public override async Task<ActionResult<IEnumerable<Project>>> GetAll()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var result = await Repository.GetAllByUserId(userId);
            return Ok(result);
        }

        [HttpGet("{projectId}/users")]
        public async Task<IEnumerable<UserDto>> GetAllProjectUsers([FromRoute] long projectId)
        {
            var users = await Repository.GetProjectUsers(projectId);
            return Mapper.Map<List<UserDto>>(users);
        }

        [HttpGet("{projectId}/sprints")]
        public async Task<IEnumerable<Sprint>> GetAllProjectSprints([FromRoute] long projectId)
        {
            return await Repository.GetProjectSprints(projectId);
        }

        [HttpPost("{projectId}/{userId}")]
        public async Task<IActionResult> AddUser([FromRoute] long projectId, [FromRoute] long userId)
        {
            try
            {
                await Repository.AddUserProject(projectId, userId);

                await UnitOfWork.Complete();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{projectId}/{userId}/{roleId}")]
        public async Task<IActionResult> ChangeRole([FromRoute] long projectId, [FromRoute] long userId, [FromRoute] long roleId)
        {
            try
            {
                var loggedUser = _currentUserService.GetCurrentUserId();
                var ChangeRole = await _projectService.EditUserRole(projectId, userId, roleId, loggedUser);
                await UnitOfWork.Complete();
                if (ChangeRole.IsSuccesfull)
                return Ok();

                return BadRequest(ChangeRole.ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public override async Task<ActionResult> Delete([FromRoute] long id)
        {
            try
            {
                var userId = _currentUserService.GetCurrentUserId();
                await _projectService.DeleteProject(id, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{projectId}/{userId}/Role")]
        public async Task<ActionResult<long>> GetUserRoleForProject([FromRoute] long projectId, [FromRoute] long userId)
        {
            try
            {
                var roleId = _projectService.GetUserRole(projectId, userId);
                return Ok(await roleId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/all")]
        public async Task<IEnumerable<Project>> GetAllUserProjects([FromBody] long userId)
        {

            var projects = _projectService.GetAllByUserId(userId);
            return await projects;
        }

        [HttpDelete("{projectId}/{userId}")]
        public async Task<IActionResult> DeleteUserFromProject([FromRoute] long projectId, [FromRoute] long userId)
        {

            var actionEffect = await _projectService.DeleteUserFromProject(projectId, userId, _currentUserService.GetCurrentUserId());
            if (actionEffect.IsSuccesfull)
            {
                return Ok();
            }

            return BadRequest(actionEffect.ErrorMessage);
        }

        [HttpPost("sendInvitation")]
        public async Task<IActionResult> SendInvitation([FromBody]InvitationDto invitationDto)
        {
            var loggedUserId = _currentUserService.GetCurrentUserId();
            if (await _roleCheck.IsAdmin(loggedUserId, invitationDto.ProjectId))
            {
                if (await _authService.UserExists(invitationDto.UserEmail))
                {
                    var user = await _authRepository.GetUserByEmailAsync(invitationDto.UserEmail);
                    try
                    {
                        await _projectService.AddUserProject(invitationDto.ProjectId, user.Id);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                    await _unitOfWork.Complete();
                    await _authService.SendTextToEmail(invitationDto.UserEmail, "Invitation",
                                           "Zostałeś dodany do nowego projektu, który znajdziesz w oknie do ich wyboru");
                    return Ok();
                }
                else
                {
                    try
                    {
                        var url = $"{_appSettings.FrontendUrl}/ProjectJoin";
                        await _authService.SendInvitationLinkToProject(invitationDto.UserEmail, invitationDto.ProjectId,
                            url);
                        return Ok();
                    }
                    catch (Exception)
                    {
                        return BadRequest("Nie udalo sie wyslac zaproszenia");
                    }
                }

            }
            return BadRequest("Brak uprawnien do tej operacji");
        }

        [HttpGet("CanLeave/{projectId}")]
        public async Task<ActionResult<Project>> CanLeave([FromRoute] long projectId)
        {
            var loggedUser = _currentUserService.GetCurrentUserId();
            try
            {
                var canLeaveProject = await _projectService.CanLeaveProject(loggedUser, projectId);
                if (canLeaveProject.IsSuccesfull)
                {
                    return Ok(null);
                }

                return Ok(await _projectRepository.Get(projectId));
            }
            catch (Exception e)
            {
                return BadRequest("Bledny Project");
            }



        }
    }
}

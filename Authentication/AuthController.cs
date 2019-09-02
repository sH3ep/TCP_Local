using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TPC.Api.Authentication.Dto;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Projects;
using TPC.Api.Users;

namespace TPC.Api.Authentication
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuthRepository _authRepository;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService,
            IAuthRepository authRepository,
            IOptions<AppSettings> appSettings,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IProjectService projectService,
            IUserService userService
        )
        {
            _authService = authService;
            _authRepository = authRepository;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _projectService = projectService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserDto loginDto)
        {
            var user = await _authService.Authenticate(loginDto.Email, loginDto.Password);

            if (user == null)
                return BadRequest(
                    new { message = "Username or password is incorrect. Remember to activate your account" });

            var tokenString = _authService.GenerateToken(user, 7, 0);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpGet("resendActivationLink/{userEmail}")]
        public async Task<IActionResult> ResendActivationLink([FromRoute] string userEmail)
        {
            try
            {
                var user =await _userService.GetUserByEmail(userEmail);
                var url = $"{_appSettings.FrontendUrl}/activate";
                await _authService.SendActivationLink(user, url);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            try
            {
                await _authService.Create(user, userDto.Password);
                var url = $"{_appSettings.FrontendUrl}/activate";
                await _authService.SendActivationLink(user, url);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("activate/{token}")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = _authService.GetPrincipal(token, _appSettings.Secret);
            }
            catch (Exception)
            {
                return BadRequest("Aktywacja konta nieudana");
            }

            if (principal == null)
                return BadRequest("Aktywacja konta nieudana");
            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return BadRequest("Aktywacja konta nieudana");
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            int.TryParse(usernameClaim.Value, out int userId);

            var user = await _authRepository.Get(userId);
            if (user != null)
            {
                user.Activated = true;
                _authRepository.Update(user);

                var userProjects = await _projectService.GetAllByUserId(user.Id);
                if (userProjects == (null) || !userProjects.Any())
                {
                    var project = new Project()
                    {
                        Title = "New Project",
                        ModificationDate = DateTime.Now,
                        ModifiedBy = userId
                    };

                    await _projectService.CreateNewProject(user.Id, project);
                }

                await _unitOfWork.Complete();
                return Ok();
            }

            return BadRequest("Aktywacja konta nieudana");

        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _authRepository.Get(id);
            return new OkObjectResult(user);
        }

        [AllowAnonymous]
        [HttpGet("deleteall")]
        public async Task DeleteAll()
        {
            var users = await _authRepository.GetAll();
            foreach (var user in users)
            {
                _authRepository.Remove(user);
            }

            await _unitOfWork.Complete();
        }

        [AllowAnonymous]
        [HttpPost("remindPassword")]
        public async Task<IActionResult> RemindPassword([FromBody] EmailDto emailDto)
        {

            User user = await _authRepository.GetUserByEmailAsync(emailDto.Email);
            if (user != null)
            {
                var url = $"{_appSettings.FrontendUrl}/remindPassword";
                await _authService.SendPasswordReset(user, url);
                return Ok("Link został wyslany na podanego maila");
            }
            return BadRequest("Konto o podanym adresie nie istnieje");
        }


        [AllowAnonymous]
        [HttpPost("remindPassword/{token}")]
        public async Task<IActionResult> ResetPassword(string token, [FromBody]PasswordDto passwordDto)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = _authService.GetPrincipal(token, _appSettings.Secret);
            }
            catch (Exception)
            {
                return BadRequest("Link nieaktywny");
            }

            if (principal == null)
                return BadRequest("Link nieaktywny");
            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return BadRequest("Link nieaktywny");
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            int.TryParse(usernameClaim.Value, out int userId);

            var user = await _authRepository.Get(userId);
            if (user != null)
            {
                await _authService.ChangePassword(user, passwordDto.Password);
                _authRepository.Update(user);
                await _unitOfWork.Complete();
                return Ok();
            }

            return BadRequest("Link nieaktywny");
        }

        [AllowAnonymous]
        [HttpPost("invRegistration/{token}")]
        public async Task<IActionResult> RegisterFromInvitation(string token, [FromBody] RegisterUserDto userDto)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = _authService.GetPrincipal(token, _appSettings.Secret);
            }
            catch (Exception)
            {
                return BadRequest("Zaproszenie uszkodzone lub nieaktualne");
            }

            if (principal == null)
                return BadRequest("Zaproszenie uszkodzone lub nieaktualne");
            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return BadRequest("Zaproszenie uszkodzone lub nieaktualne");
            }


            var claims = identity.FindAll(ClaimTypes.Name).ToList();
            Claim userNameClaim = claims[0];
            Claim projectIdClaim = claims[1];

            long.TryParse(projectIdClaim.Value, out long projectId);

            var user = _mapper.Map<User>(userDto);
            user.Email = userNameClaim.Value;
            if (!(await _authService.UserExists(user.Email)))
            {
                try
                {
                    await _authService.Create(user, userDto.Password);
                    await _unitOfWork.Complete();
                    await _projectService.AddUserProject(projectId, user.Id);
                    var url = $"{_appSettings.FrontendUrl}/activate";
                    await _authService.SendActivationLink(user, url);
                    return Ok();
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }

            }

            return BadRequest("Taki uzytkownik juz istnieje");


        }
    }
}

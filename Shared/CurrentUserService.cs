using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TPC.Api.Model;
using TPC.Api.Users;

namespace TPC.Api.Shared
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetCurrentUserId()
        {
            return long.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public Task<User> GetCurrentUser()
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            return _userRepository.Get(userId);
        }
    }
}

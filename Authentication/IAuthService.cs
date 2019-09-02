using System.Security.Claims;
using System.Threading.Tasks;
using TPC.Api.Model;

namespace TPC.Api.Authentication
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        Task SendActivationLink(User user, string url);
        Task SendPasswordReset(User user, string url);
        Task<User> ChangePassword(User user, string password);
        ClaimsPrincipal GetPrincipal(string token, string secret);
        string GenerateToken(User user, int expiresDays,int expireHours);
        Task SendInvitationLinkToProject(string userEmail, long projectId, string url);
        Task<bool> UserExists(string userEmail);
        Task SendTextToEmail(string userEmail, string subject, string messageText);
    }
}

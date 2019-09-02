using System.Threading.Tasks;

namespace TPC.Api.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
}

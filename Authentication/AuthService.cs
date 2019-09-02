using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TPC.Api.EmailService;
using TPC.Api.Model;
using TPC.Api.Persistence;

namespace TPC.Api.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public AuthService(IAuthRepository authRepository, IUnitOfWork unitOfWork, IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;


            var user = await _authRepository.GetUserByEmailAsync(username);

            if (user == null)
                return null;

            if (!user.Activated)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (await _authRepository.GetUserByEmailAsync(user.Email) != null)
                throw new Exception("Username \"" + user.Email + "\" is already taken");

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _authRepository.Add(user);
            await _unitOfWork.Complete();

            return user;
        }

        public async Task<bool> UserExists(string userEmail)
        {
            if (await _authRepository.GetUserByEmailAsync(userEmail) != null)
            {
                return true;
            }

            return false;



        }



        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public ClaimsPrincipal GetPrincipal(string token, string secret)
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            if (jwtToken == null)
                return null;
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                parameters, out _);
            return principal;

        }

        public string GenerateToken(User user, int expiresDays, int expireHours)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(expiresDays).AddHours(expireHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task SendInvitationLinkToProject(string userEmail, long projectId, string url)
        {
            var token = GenerateInvitationToken(userEmail, projectId, 12);
            await _emailService.SendEmail(userEmail, "Project Invitation", $"{url}/{token}");
        }

        public async Task SendTextToEmail(string userEmail, string subject,string messageText)
        {
           
            await _emailService.SendEmail(userEmail, subject, messageText);
        }

        public async Task SendActivationLink(User user, string url)
        {
            var token = GenerateToken(user, 7, 0);
            await _emailService.SendEmail(user.Email, "Activate your account", $"{url}/{token}");
        }

        public async Task SendPasswordReset(User user, string url)
        {
            var token = GenerateToken(user, 0, 1);
            await _emailService.SendEmail(user.Email, "To reset your password use that link: ", $"{url}/{token}");
        }

        public async Task<User> ChangePassword(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _unitOfWork.Complete();

            return user;
        }

        private string GenerateInvitationToken(string email, long projectId, int expireHours)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Name, projectId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(expireHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

       

    }
}

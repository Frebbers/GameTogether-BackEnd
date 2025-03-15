using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameTogetherAPI.Models;
using GameTogetherAPI.Repository;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace GameTogetherAPI.Services
{
    /// <summary>
    /// Provides authentication and user management services, including registration, login, and token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for user-related database operations.</param>
        /// <param name="configuration">The configuration settings for authentication.</param>
        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Registers a new user by hashing their password and storing their credentials.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The plaintext password to be hashed and stored.</param>
        /// <returns>A task representing the asynchronous operation, returning true if registration is successful, otherwise false.</returns>
        public async Task<bool> RegisterUserAsync(string email, string password)
        {
            if (await _userRepository.GetUserByEmailAsync(email) != null)
                return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { Email = email, PasswordHash = hashedPassword };

            await _userRepository.AddUserAsync(user);
            return true;
        }

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the user is successfully deleted.</returns>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteUserAsync(userId);
            return true;
        }


        public async Task<bool> SendEmailVerificationAsync(string email) {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            var token = GenerateEmailVerificationToken(user.Id);

            var smtpSettings = _configuration.GetSection("EmailSettings");
            var verificationUrl = smtpSettings["VerificationUrl"] + $"?token={token}";
            var smtpServer = smtpSettings["SmtpServer"];
            var port = int.Parse(smtpSettings["Port"]);
            var senderEmail = smtpSettings["SenderEmail"];
            var senderPassword = smtpSettings["SenderPassword"];

            // **Correct the file path to match the Models folder**
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "email-template.html");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Email template not found at {templatePath}");

            string emailBody = await File.ReadAllTextAsync(templatePath);
            emailBody = emailBody.Replace("{VERIFICATION_URL}", verificationUrl);

            using (var client = new SmtpClient(smtpServer)) {
                client.Port = port;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage {
                    From = new MailAddress(senderEmail),
                    Subject = "Verify Your Email",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                await client.SendMailAsync(mailMessage);
            }

            return true;
        }



        /// <summary>
        /// Generates a JWT token for email verification.
        /// </summary>
        private string GenerateEmailVerificationToken(int userId) {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("email_verification", "true") // Custom claim
            };

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Verifies the email using a JWT token.
        /// </summary>
        public async Task<bool> ConfirmEmailAsync(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

            try {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var emailVerificationClaim = principal.FindFirst("email_verification")?.Value;

                if (emailVerificationClaim != "true")
                    return false;

                return await _userRepository.ConfirmEmailAsync(userId);
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Authenticates a user by verifying their credentials and returning a JWT token if valid.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The plaintext password provided for authentication.</param>
        /// <returns>A task representing the asynchronous operation, returning a JWT token if authentication is successful, otherwise null.</returns>
        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            if (!user.IsEmailVerified)
                return null;

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Generates a JWT token for an authenticated user.
        /// </summary>
        /// <param name="user">The authenticated user for whom the token is generated.</param>
        /// <returns>A string representing the generated JWT token.</returns>
        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

}

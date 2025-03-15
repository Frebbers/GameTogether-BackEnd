using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameTogetherAPI.Models;
using GameTogetherAPI.Services;
using System.Security.Claims;

namespace GameTogetherAPI.Controllers {
    [Route("api/auth")]
    [ApiController]
    /// <summary>
    /// Handles user authentication, including registration, login, and user account management.
    /// </summary>
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service used to handle user authentication and management.</param>
        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided email and password and sends a verification email.
        /// </summary>
        /// <param name="model">The registration model containing the user's email and password.</param>
        /// <returns>
        /// Returns a 200 OK response if registration is successful and the verification email is sent.
        /// Returns a 400 Bad Request response if the email is already taken or if the verification email could not be sent.
        /// </returns>
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model) {
            bool success = await _authService.RegisterUserAsync(model.Email, model.Password);
            if (!success)
                return BadRequest("Email already taken.");

            bool emailSent = await _authService.SendEmailVerificationAsync(model.Email);
            if (!emailSent)
                return BadRequest("Could not send verification email.");

            return Ok("User registered successfully!");
        }

        /// <summary>
        /// Confirms the user's email using the verification token.
        /// </summary>
        /// <param name="token">JWT verification token</param>
        /// <returns>200 OK if successful, 400 Bad Request otherwise</returns>
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token) {
            bool success = await _authService.ConfirmEmailAsync(token);
            if (!success)
                return BadRequest("Invalid or expired verification token.");

            return Ok("Email verified successfully.");
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="model">The login model containing the user's email and password.</param>
        /// <returns>
        /// Returns a 200 OK response with a JWT token if authentication is successful.
        /// Returns a 401 Unauthorized response if credentials are invalid.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {
            var token = await _authService.AuthenticateUserAsync(model.Email, model.Password);
            if (token == null)
                return Unauthorized("Invalid credentials or your user has not been verified.");

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Deletes the currently authenticated user's account.
        /// </summary>
        /// <returns>
        /// Returns a 200 OK response if the account is successfully deleted.
        /// Returns a 400 Bad Request response if the deletion fails.
        /// </returns>
        [HttpDelete("remove-user/")]
        [Authorize]
        public async Task<IActionResult> DeleteUser() {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool success = await _authService.DeleteUserAsync(userId);

            if (!success)
                return BadRequest(new { message = "Unable to delete user." });

            return Ok();
        }

        /// <summary>
        /// Retrieves the currently authenticated user's email.
        /// </summary>
        /// <returns>
        /// Returns a 200 OK response with the user's email.
        /// </returns>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser() {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { Email = userEmail });
        }
    }
}
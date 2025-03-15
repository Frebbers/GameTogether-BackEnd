using GameTogetherAPI.DTO;
using GameTogetherAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameTogetherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Handles user-related operations, including retrieving and updating user profiles.
    /// </summary>
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service responsible for handling user-related operations.</param>

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves the profile of the authenticated user.
        /// </summary>
        /// <returns>
        /// Returns a 200 OK response with the user's profile.  
        /// Returns a 404 Not Found response if the user does not exist.
        /// </returns>

        [HttpGet("get-profile")]
        [Authorize]
        public async Task<IActionResult> GetProfileAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var profile = await _userService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound(new { message = "User not found" });

            return Ok(profile);
        }

        /// <summary>
        /// Updates or creates the profile of the authenticated user.
        /// </summary>
        /// <param name="profile">The profile data to be updated.</param>
        /// <returns>
        /// Returns a 200 OK response if the profile is successfully updated.  
        /// Returns a 400 Bad Request response if the update fails.  
        /// Returns a 500 Internal Server Error response if an unexpected error occurs.
        /// </returns>

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDTO profile)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                bool success = await _userService.AddOrUpdateProfileAsync(userId, profile);

                if (!success)
                    return BadRequest(new { message = "Profile creation failed due to a bad request" });

                return Ok(new { message = "Profile updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

    }
}

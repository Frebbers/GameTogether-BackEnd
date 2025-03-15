using GameTogetherAPI.DTO;
using GameTogetherAPI.Models;
using GameTogetherAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameTogetherAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Manages game sessions, including session creation, retrieval, joining, and leaving.
    /// </summary>
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionsController"/> class.
        /// </summary>
        /// <param name="sessionservice">The session service responsible for handling session-related operations.</param>

        public SessionsController(ISessionService sessionservice)
        {
            _sessionService = sessionservice; 
        }

        /// <summary>
        /// Creates a new session for the authenticated user.
        /// </summary>
        /// <param name="sessionDto">The session details provided in the request body.</param>
        /// <returns>
        /// Returns a 201 Created response if the session is successfully created.  
        /// Returns a 400 Bad Request response if the session creation fails.
        /// </returns>

        [HttpPost("create")]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequestDTO sessionDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool success = await _sessionService.CreateSessionAsync(userId, sessionDto);

            if (!success)
                return BadRequest(new { message = "Failed to create session." });

            return Created(string.Empty, new { message = "Session created successfully!" });
        }

        /// <summary>
        /// Retrieves all available sessions.
        /// </summary>
        /// <returns>
        /// Returns a 200 OK response with a list of sessions.  
        /// Returns a 404 Not Found response if no sessions are available.
        /// </returns>

        [HttpGet]
        public async Task<IActionResult> GetSessionsAsync()
        {
            var sessions = await _sessionService.GetSessionsAsync();

            if (sessions == null)
                return NotFound(new { message = "Sessions not found" });

            return Ok(sessions);
        }

        /// <summary>
        /// Retrieves a session by its unique identifier.
        /// </summary>
        /// <param name="sessionId">The unique identifier of the session.</param>
        /// <returns>
        /// A 200 OK response containing the session details if found.  
        /// A 404 Not Found response if the session does not exist.
        /// </returns>

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSessionByIdAsync(int sessionId) {
            var session = await _sessionService.GetSessionByIdAsync(sessionId);

            if (session == null)
                return NotFound(new { message = "Session not found" });

            return Ok(session);
        }

        /// <summary>
        /// Retrieves all sessions that the authenticated user is participating in.
        /// </summary>
        /// <returns>
        /// A 200 OK response containing the list of sessions the user is part of.  
        /// A 404 Not Found response if no sessions are found.
        /// </returns>

        [HttpGet("user")]
        public async Task<IActionResult> GetMySessionsAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var sessions = await _sessionService.GetSessionsAsync(userId);

            if (sessions == null)
                return NotFound(new { message = "Sessions not found" });

            return Ok(sessions);
        }

        /// <summary>
        /// Allows the authenticated user to join a specified session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to join.</param>
        /// <returns>
        /// Returns a 200 OK response if the user successfully joins the session.  
        /// Returns a 400 Bad Request response if the session does not exist or the user is already a participant.
        /// </returns>

        [HttpPost("{sessionId}/join")]
        public async Task<IActionResult> JoinSession(int sessionId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool success = await _sessionService.JoinSessionAsync(userId, sessionId);

            if (!success)
                return BadRequest(new { message = "Failed to join session. Either it does not exist or you're already a participant." });

            return Ok(new { message = "Successfully joined the session!" });
        }

        /// <summary>
        /// Allows the authenticated user to leave a specified session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to leave.</param>
        /// <returns>
        /// Returns a 200 OK response if the user successfully leaves the session.  
        /// Returns a 400 Bad Request response if the session does not exist or the user has already left.
        /// </returns>

        [HttpDelete("{sessionId}/leave")]
        public async Task<IActionResult> LeaveSession(int sessionId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool success = await _sessionService.LeaveSessionAsync(userId, sessionId);

            if (!success)
                return BadRequest(new { message = "Failed to leave session. Either it does not exist or you've already left." });

            return Ok(new { message = "Successfully left the session!" });
        }

    }
}

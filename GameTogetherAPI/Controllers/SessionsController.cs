﻿using GameTogetherAPI.DTO;
using GameTogetherAPI.Models;
using GameTogetherAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameTogetherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService sessionservice)
        {
            _sessionService = sessionservice; 
        }

        [HttpPost("create-session")]
        [Authorize]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequestDTO sessionDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool success = await _sessionService.CreateSessionAsync(userId, sessionDto);

            if (!success)
                return BadRequest(new { message = "Failed to create session." });

            return Created(string.Empty, new { message = "Session created successfully!" });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSessionsAsync()
        {
            var sessions = await _sessionService.GetSessionsAsync();

            if (sessions == null)
                return NotFound(new { message = "Sessions not found" });

            return Ok(sessions);
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMySessionsAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var sessions = await _sessionService.GetSessionsAsync(userId);

            if (sessions == null)
                return NotFound(new { message = "Sessions not found" });

            return Ok(sessions);
        }

    }
}

using GameTogetherAPI.DTO;
using GameTogetherAPI.Models;

namespace GameTogetherAPI.Services
{
    /// <summary>
    /// Defines the contract for session management services.
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Creates a new session for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user creating the session.</param>
        /// <param name="session">The session details provided in the request.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the session is successfully created.</returns>
        Task<bool> CreateSessionAsync(int userId , CreateSessionRequestDTO session);
        
        Task<GetSessionByIdResponseDTO> GetSessionByIdAsync(int sessionId);

        /// <summary>
        /// Retrieves all available sessions or sessions associated with a specific user.
        /// </summary>
        /// <param name="userId">The optional user ID to filter sessions by user participation. If null, all sessions are retrieved.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of available sessions.</returns>
        Task<List<GetSessionsResponseDTO>> GetSessionsAsync(int? userId = null);

        /// <summary>
        /// Allows a user to join a specified session.
        /// </summary>
        /// <param name="userId">The unique identifier of the user joining the session.</param>
        /// <param name="sessionId">The unique identifier of the session to join.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the user successfully joins the session.</returns>
        Task<bool> JoinSessionAsync(int userId, int sessionId);

        /// <summary>
        /// Allows a user to leave a specified session.
        /// </summary>
        /// <param name="userId">The unique identifier of the user leaving the session.</param>
        /// <param name="sessionId">The unique identifier of the session to leave.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the user successfully leaves the session.</returns>
        Task<bool> LeaveSessionAsync(int userId, int sessionId);
    }
}

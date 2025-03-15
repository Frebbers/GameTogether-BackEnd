using GameTogetherAPI.Models;

namespace GameTogetherAPI.Repository {
    /// <summary>
    /// Defines the contract for session-related database operations.
    /// </summary>
    public interface ISessionRepository {
        /// <summary>
        /// Creates a new session asynchronously.
        /// </summary>
        /// <param name="session">The session to create.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the session is successfully created.</returns>
        Task<bool> CreateSessionAsync(Session session);

        /// <summary>
        /// Adds a user to a session asynchronously.
        /// </summary>
        /// <param name="userSession">The user-session relationship to add.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the user is successfully added to the session.</returns>
        Task<bool> AddUserToSessionAsync(UserSession userSession);

        /// <summary>
        /// Retrieves all available sessions asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, returning a list of all sessions.</returns>
        Task<List<Session>> GetSessionsAsync();

        /// <summary>
        /// Retrieves all sessions that a specific user is participating in asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation, returning a list of sessions the user is part of.</returns>
        Task<List<Session>> GetSessionsByUserIdAsync(int userId);

        /// <summary>
        /// Validates whether a user is part of a specific session.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="sessionId">The unique identifier of the session.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the user is part of the session.</returns>
        Task<bool> ValidateUserSessionAsync(int userId, int sessionId);

        /// <summary>
        /// Removes a user from a session asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="sessionId">The unique identifier of the session.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the user is successfully removed from the session.</returns>
        Task<bool> RemoveUserFromSessionAsync(int userId, int sessionId);
        Task<Session> GetSessionByIdAsync(int sessionId);
    }
}

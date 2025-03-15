using GameTogetherAPI.Database;
using GameTogetherAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTogetherAPI.Repository {
    /// <summary>
    /// Handles database operations related to game sessions.
    /// </summary>
    public class SessionRepository : ISessionRepository {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for interacting with sessions.</param>
        public SessionRepository(ApplicationDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Creates a new session in the database.
        /// </summary>
        /// <param name="session">The session to be created.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if successful, otherwise false.</returns>
        public async Task<bool> CreateSessionAsync(Session session) {
            try {
                await _context.Sessions.AddAsync(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException) {
                return false;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Retrieves a session by its unique identifier, including its participants and their profiles.
        /// </summary>
        /// <param name="sessionId">The unique identifier of the session.</param>
        /// <returns>
        /// A task representing the asynchronous operation, returning the session if found, otherwise null.
        /// </returns>
        public async Task<Session> GetSessionByIdAsync(int sessionId) {
            return await _context.Sessions
                .Include(s => s.Participants)
                .ThenInclude(p => p.User)
                .ThenInclude(u => u.Profile).FirstOrDefaultAsync();

        }

        /// <summary>
        /// Adds a user to a session and saves the change to the database.
        /// </summary>
        /// <param name="userSession">The user-session relationship to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, returning true if the user is successfully added to the session.
        /// </returns>
        public async Task<bool> AddUserToSessionAsync(UserSession userSession)
        {
            await _context.UserSessions.AddAsync(userSession);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Removes a user from a session.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="sessionId">The unique identifier of the session.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the user is successfully removed, otherwise false.</returns>
        public async Task<bool> RemoveUserFromSessionAsync(int userId, int sessionId) {
            var userSession = await _context.UserSessions
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SessionId == sessionId);

            if (userSession == null)
                return false;

            _context.UserSessions.Remove(userSession);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Validates whether a user is not already a participant in a session.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="sessionId">The unique identifier of the session.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the user is not already a participant, otherwise false.</returns>
        public async Task<bool> ValidateUserSessionAsync(int userId, int sessionId) {
            bool sessionExists = await _context.Sessions.AnyAsync(s => s.Id == sessionId);
            if (!sessionExists)
                return false;

            bool isParticipant = await _context.UserSessions
                .AnyAsync(us => us.UserId == userId && us.SessionId == sessionId);

            return !isParticipant;
        }

        /// <summary>
        /// Retrieves all sessions a user is participating in.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation, returning a list of sessions the user is part of.</returns>
        public async Task<List<Session>> GetSessionsByUserIdAsync(int userId) {
            return await _context.Sessions
                .Where(s => s.Participants.Any(p => p.UserId == userId))
                .Include(s => s.Participants)
                .ThenInclude(p => p.User)
                .ThenInclude(u => u.Profile)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all available sessions.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, returning a list of all sessions.</returns>
        public async Task<List<Session>> GetSessionsAsync() {
            return await _context.Sessions
                .Include(s => s.Participants)
                .ThenInclude(p => p.User)
                .ThenInclude(u => u.Profile)
                .ToListAsync();
        }
    }
}

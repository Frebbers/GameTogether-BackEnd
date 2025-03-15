using GameTogetherAPI.DTO;
using GameTogetherAPI.Models;
using GameTogetherAPI.Repository;

namespace GameTogetherAPI.Services
{
    /// <summary>
    /// Provides session management services, including session creation, retrieval, joining, and leaving.
    /// </summary>
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        /// <summary>
        /// Provides session management services, including session creation, retrieval, joining, and leaving.
        /// </summary>
        public SessionService(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// Creates a new session and assigns the user as its owner.
        /// </summary>
        /// <param name="userId">The unique identifier of the user creating the session.</param>
        /// <param name="sessionDto">The session details provided in the request.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the session is successfully created.</returns>
        public async Task<bool> CreateSessionAsync(int userId, CreateSessionRequestDTO sessionDto)
        {
            var session = new Session()
            {
                Title = sessionDto.Title,
                AgeRange = sessionDto.AgeRange,
                Description = sessionDto.Description,
                IsVisible = sessionDto.IsVisible,
                OwnerId = userId,
                Tags = sessionDto.Tags,

            };

            bool isSuccess = await _sessionRepository.CreateSessionAsync(session);

            if (!isSuccess)
                return false;

            var userSession = new UserSession
            {
                UserId = userId,
                SessionId = session.Id
            };

            await _sessionRepository.AddUserToSessionAsync(userSession);

            return true;

        }

        /// <summary>
        /// Retrieves a session by its unique identifier and maps it to a response DTO.
        /// </summary>
        /// <param name="sessionId">The unique identifier of the session.</param>
        /// <returns>
        /// A task representing the asynchronous operation, returning a <see cref="GetSessionByIdResponseDTO"/> 
        /// containing session details if found.
        /// </returns>
        public async Task<GetSessionByIdResponseDTO> GetSessionByIdAsync(int sessionId) {
            var session = await _sessionRepository.GetSessionByIdAsync(sessionId);

            return new GetSessionByIdResponseDTO() {
                Title = session.Title,
                AgeRange = session.AgeRange,
                Description = session.Description,
                IsVisible = session.IsVisible,
                OwnerId = sessionId,
                Tags = session.Tags,
                Id = sessionId,
                Participants = session.Participants
                    .Select(p => new ParticipantDTO {
                        UserId = p.UserId,
                        Name = p.User.Profile.Name,
                    })
                    .ToList()
            };
        }

        /// <summary>
        /// Retrieves all available sessions or sessions associated with a specific user.
        /// </summary>
        /// <param name="userId">The optional user ID to filter sessions by user participation. If null, all sessions are retrieved.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of available sessions.</returns>
        public async Task<List<GetSessionsResponseDTO>> GetSessionsAsync(int? userId = null)
        {
        
            var sessions = userId == null
                ? await _sessionRepository.GetSessionsAsync() 
                : await _sessionRepository.GetSessionsByUserIdAsync((int)userId);

            if (sessions == null)
                return null;

            var results = new List<GetSessionsResponseDTO>();

            foreach (var session in sessions)
            {
                results.Add(new GetSessionsResponseDTO
                {
                    Id = session.Id,
                    Title = session.Title,
                    OwnerId = session.OwnerId,
                    IsVisible = session.IsVisible,
                    AgeRange = session.AgeRange,
                    Description = session.Description,
                    Tags = session.Tags,
                    Participants = session.Participants
                    .Select(p => new ParticipantDTO
                    {
                        UserId = p.UserId,
                        Name = p.User.Profile.Name,
                    })
                    .ToList()
                });
            }
            return results; 
        }

        /// <summary>
        /// Allows a user to join a specified session if they are not already a participant.
        /// </summary>
        /// <param name="userId">The unique identifier of the user joining the session.</param>
        /// <param name="sessionId">The unique identifier of the session to join.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the user successfully joins the session.</returns>
        public async Task<bool> JoinSessionAsync(int userId, int sessionId)
        {

            if (!await _sessionRepository.ValidateUserSessionAsync(userId, sessionId))
                return false;

            var userSession = new UserSession
            {
                UserId = userId,
                SessionId = sessionId
            };

            await _sessionRepository.AddUserToSessionAsync(userSession);

            return true;
        }

        /// <summary>
        /// Allows a user to leave a specified session.
        /// </summary>
        /// <param name="userId">The unique identifier of the user leaving the session.</param>
        /// <param name="sessionId">The unique identifier of the session to leave.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the user successfully leaves the session.</returns>
        public async Task<bool> LeaveSessionAsync(int userId, int sessionId)
        {
            return await _sessionRepository.RemoveUserFromSessionAsync(userId, sessionId);
            
        }

    }
}

namespace GameTogetherAPI.Models {
    /// <summary>
    /// Represents the relationship between a user and a session, indicating that a user has joined a session.
    /// </summary>
    public class UserSession {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the session.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the session.
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        /// Gets or sets the session associated with the user.
        /// </summary>
        public Session Session { get; set; }
    }
}

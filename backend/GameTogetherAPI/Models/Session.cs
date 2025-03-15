using System.ComponentModel.DataAnnotations;

namespace GameTogetherAPI.Models {
    /// <summary>
    /// Represents a game session that users can join.
    /// </summary>
    public class Session {
        /// <summary>
        /// Gets or sets the unique identifier of the session.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the session.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the session owner.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the user who owns the session.
        /// </summary>
        public User Owner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the session is visible to others.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the recommended age range for participants.
        /// </summary>
        public string AgeRange { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the session.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of users participating in the session.
        /// </summary>
        public List<UserSession> Participants { get; set; } = new();

        /// <summary>
        /// Gets or sets a list of tags associated with the session for filtering and categorization.
        /// </summary>
        public List<string> Tags { get; set; } = new();
    }
}

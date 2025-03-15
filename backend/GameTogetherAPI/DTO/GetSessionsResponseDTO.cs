using GameTogetherAPI.Models;

namespace GameTogetherAPI.DTO {
    /// <summary>
    /// Represents the response data for retrieving a session.
    /// </summary>
    public class GetSessionsResponseDTO {
        /// <summary>
        /// Gets or sets the unique identifier of the session.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the session.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the session owner.
        /// </summary>
        public int OwnerId { get; set; }

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
        /// Gets or sets a list of tags associated with the session for filtering and categorization.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the list of participants in the session.
        /// </summary>
        public List<ParticipantDTO> Participants { get; set; } = new();
    }
}

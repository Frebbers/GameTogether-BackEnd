namespace GameTogetherAPI.DTO
{
    /// <summary>
    /// Represents the data required to create a new game session.
    /// </summary>
    public class CreateSessionRequestDTO
    {
        /// <summary>
        /// The title of the session.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Indicates whether the session is visible to others.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// The recommended age range for participants.
        /// </summary>
        public string AgeRange { get; set; }

        /// <summary>
        /// A brief description of the session.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A list of tags associated with the session for filtering and categorization.
        /// </summary>
        public List<string> Tags { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameTogetherAPI.Models {
    /// <summary>
    /// Represents a user's profile containing personal details and preferences.
    /// </summary>
    public class Profile {
        /// <summary>
        /// Gets or sets the unique identifier of the profile.
        /// This also serves as a foreign key to the corresponding <see cref="User"/>.
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the URL or base64 string of the user's profile picture.
        /// </summary>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// Gets or sets a brief description or bio of the user.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the region or location of the user.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this profile.
        /// </summary>
        public User User { get; set; }
    }
}

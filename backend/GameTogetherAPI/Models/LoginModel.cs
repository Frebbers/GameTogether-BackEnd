using System.ComponentModel.DataAnnotations;

namespace GameTogetherAPI.Models {
    /// <summary>
    /// Represents the data required for user login.
    /// </summary>
    public class LoginModel {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user account.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace GameTogetherAPI.Models {
    /// <summary>
    /// Represents the data required for user registration.
    /// </summary>
    public class RegisterModel {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user account.
        /// </summary>
        [Required]
        public string Password { get; set; }
        public RegisterModel(string email, string password) {
            Email = email;
            Password = password;
        }
    }
}

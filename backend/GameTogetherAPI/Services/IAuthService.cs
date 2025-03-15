using GameTogetherAPI.Models;

namespace GameTogetherAPI.Services
{
    /// <summary>
    /// Defines the contract for authentication and user management services.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the given email and password.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The plaintext password to be stored securely.</param>
        /// <returns>A task representing the asynchronous operation, returning true if registration is successful, otherwise false.</returns>
        Task<bool> RegisterUserAsync(string email, string password);

        /// <summary>
        /// Sends an email verification link to the user's email address.
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <returns>Task representing the async operation</returns>
        Task<bool> SendEmailVerificationAsync(string email);

        /// <summary>
        /// Confirms the user's email using a verification token.
        /// </summary>
        /// <param name="token">JWT verification token</param>
        /// <returns>Task representing the async operation</returns>
        Task<bool> ConfirmEmailAsync(string token);

        /// <summary>
        /// Authenticates a user by verifying their credentials and returning a JWT token if valid.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The plaintext password provided for authentication.</param>
        /// <returns>A task representing the asynchronous operation, returning a JWT token if authentication is successful, otherwise null.</returns>
        Task<string> AuthenticateUserAsync(string email, string password);

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the user is successfully deleted.</returns>
        Task<bool> DeleteUserAsync(int userId);
    }
}

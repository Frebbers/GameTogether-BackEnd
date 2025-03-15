using GameTogetherAPI.Models;

namespace GameTogetherAPI.Repository {
    /// <summary>
    /// Defines the contract for user-related database operations.
    /// </summary>
    public interface IUserRepository {
        /// <summary>
        /// Adds a new user asynchronously.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the user is successfully added.</returns>
        Task<bool> AddUserAsync(User user);

        /// <summary>
        /// Adds or updates a user's profile asynchronously.
        /// </summary>
        /// <param name="profile">The profile to be added or updated.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the profile is successfully added or updated.</returns>
        Task<bool> AddOrUpdateProfileAsync(Profile profile);

        /// <summary>
        /// Retrieves a user by their email asynchronously.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A task that represents the asynchronous operation, returning the user if found, otherwise null.</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Confirms a user's email address.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Task representing the async operation</returns>
        Task<bool> ConfirmEmailAsync(int userId);

        /// <summary>
        /// Retrieves a user's profile asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation, returning the user's profile if found, otherwise null.</returns>
        Task<Profile> GetProfileAsync(int userId);

        /// <summary>
        /// Deletes a user and their associated data asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the user is successfully deleted.</returns>
        Task<bool> DeleteUserAsync(int userId);
    }
}

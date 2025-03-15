using GameTogetherAPI.DTO;
using GameTogetherAPI.Models;

namespace GameTogetherAPI.Services
{
    /// <summary>
    /// Defines the contract for user profile management services.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves the profile information of a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation, returning the user's profile details.</returns>
        Task<GetProfileResponseDTO> GetProfileAsync(int userId);

        /// <summary>
        /// Adds or updates the profile of a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="profileDto">The profile data to be added or updated.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the operation is successful.</returns>
        Task<bool> AddOrUpdateProfileAsync(int userId, UpdateProfileRequestDTO profileDto);
    }
}
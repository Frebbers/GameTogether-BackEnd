﻿using GameTogetherAPI.Models;

namespace GameTogetherAPI.Repository
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> AddOrUpdateProfileAsync(Profile profile);
        Task<User> GetUserByEmailAsync(string email);
        Task<Profile> GetProfileAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
    }
}

using GameTogetherAPI.Database;
using GameTogetherAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTogetherAPI.Repository
{
    /// <summary>
    /// Handles database operations related to users and their profiles.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for interacting with users and profiles.</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        /// <returns>A task representing the asynchronous operation, returning true if successful, otherwise false.</returns>
        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateException ex) 
            {
                return false;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        /// <summary>
        /// Adds or updates a user's profile in the database.
        /// </summary>
        /// <param name="profile">The profile to add or update.</param>
        /// <returns>A task representing the asynchronous operation, returning true if successful, otherwise false.</returns>
        public async Task<bool> AddOrUpdateProfileAsync(Profile profile)
        {
            try
            {
                var existingProfile = await _context.Profiles.FindAsync(profile.Id);

                if (existingProfile == null)
                {
                    await _context.Profiles.AddAsync(profile);
                }
                else
                {
                    _context.Entry(existingProfile).CurrentValues.SetValues(profile);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A task representing the asynchronous operation, returning the user if found, otherwise null.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Confirms a user's email verification status.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the email was successfully verified.</returns>
        public async Task<bool> ConfirmEmailAsync(int userId) {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsEmailVerified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a user and their associated data from the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, returning true if successful, otherwise false.</returns>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
                return false;


            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a user's profile by their user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation, returning the profile if found, otherwise null.</returns>
        public async Task<Profile> GetProfileAsync(int userId)
        {
            return await _context.Profiles.FindAsync(userId);
        }
    }
}



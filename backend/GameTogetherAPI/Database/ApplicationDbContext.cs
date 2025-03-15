using GameTogetherAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTogetherAPI.Database {
    /// <summary>
    /// Represents the database context for the application, handling entity configurations and database interactions.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified database options.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Represents the users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Represents user profiles associated with users in the database.
        /// </summary>
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Represents sessions available in the database.
        /// </summary>
        public DbSet<Session> Sessions { get; set; }

        /// <summary>
        /// Represents the many-to-many relationship between users and sessions.
        /// </summary>
        public DbSet<UserSession> UserSessions { get; set; }

        /// <summary>
        /// Configures entity relationships and constraints using the Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to define entity relationships.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSession>()
                .HasKey(us => new { us.UserId, us.SessionId });

            modelBuilder.Entity<UserSession>()
                .HasOne(us => us.User)
                .WithMany(u => u.JoinedSessions)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSession>()
                .HasOne(us => us.Session)
                .WithMany(s => s.Participants)
                .HasForeignKey(us => us.SessionId);
        }
    }

}

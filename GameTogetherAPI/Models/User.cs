﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;

namespace GameTogetherAPI.Models {
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        public bool IsEmailVerified { get; set; } = false;

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the profile associated with the user.
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// Gets or sets the list of groups the user has joined.
        /// </summary>
        public List<UserGroup> JoinedGroups { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of messages sent by the user.
        /// </summary>
        public List<Message> SentMessages { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of chats the user is participating in.
        /// </summary>
        public List<UserChat> Chats { get; set; } = new();
    }
}

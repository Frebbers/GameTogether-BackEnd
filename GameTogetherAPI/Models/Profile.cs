﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameTogetherAPI.Models
{
    public class Profile
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Description { get; set; }
        public string? Region { get; set; }
        public List<string> Tags { get; set; } = new();

        public User User { get; set; }
    }
}

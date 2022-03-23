﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Inventory_API.Data.Entities
{
    public class User
    {
        [Key]
        [MaxLength(32)] [Required] public string Username { get; set; }
        [Required] [JsonIgnore] public string Password { get; set; }
        public string Role { get; set; }
        public ICollection<User> Friends { get; set; }
        [InverseProperty("Author")] public ICollection<Room> Rooms { get; set; }
        public ICollection<List> Lists { get; set; }
    }
}

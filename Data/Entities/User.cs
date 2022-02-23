using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory_API.Data.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        [JsonIgnore] public string Password { get; set; }
        public string Role { get; set; }
        public ICollection<User> Friends { get; set; }
        [InverseProperty("Author")]
        public ICollection<Room> Rooms { get; set; }
        public ICollection<List> Lists { get; set; }
    }
}

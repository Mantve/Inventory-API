using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory_API.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [JsonIgnore] public string Password { get; set; }
        public string Role { get; set; }
    }
}

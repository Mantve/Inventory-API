using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Entities
{
    public class Room
    {
        public int Id { get; set; }
        [MaxLength(32)] [Required] public string Name { get; set; }
        public ICollection<User> SharedWith { get; set; }
        [Required] public User Author { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}

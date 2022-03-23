using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Entities
{
    public class List //shopping list, maintenance list, etc
    {
        public int Id { get; set; }
        [MaxLength(32)] [Required] public string Name { get; set; }
        public ICollection<ListItem> Items { get; set; }
        [Required] public User Author { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Entities
{
    public class List //shopping list, maintenance list, etc
    {
        public int Id { get; set; }
        [MaxLength(64)] [Required] public string Name { get; set; }
        public ICollection<ListItem> Items { get; set; }
        [DefaultValue(0)] public int ItemCount { get; set; }
        [Required] public User Author { get; set; }
    }
}

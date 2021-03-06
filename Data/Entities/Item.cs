using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Entities
{
    public class Item
    {
        public int Id { get; set; }
        [MaxLength(64)] [Required] public string Name { get; set; }
        [DefaultValue(1)] public float Quantity { get; set; }
        public ICollection<Item> Items { get; set; }
        [DefaultValue(0)] public decimal Value { get; set; }
        [Required] public Category Category { get; set; }
        [MaxLength(256)] public string Comments { get; set; }
        [Required] public Room Room { get; set; }
        public int Level { get; set; }
        public Item ParentItem { get; set; }
        [DefaultValue(false)] public bool Sold { get; set; }
    }
}

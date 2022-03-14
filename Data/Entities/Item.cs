using System.Collections.Generic;

namespace Inventory_API.Data.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public ICollection<Item> Items { get; set; }
        public decimal Value { get; set; }
        public Category Category { get; set; }
        public User Author { get; set; }
        public string Comments { get; set; }
        public Room Room { get; set; }
    }
}

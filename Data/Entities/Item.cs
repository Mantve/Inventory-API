using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public ICollection<Item> Items { get; set; }
        public decimal Value { get; set; }
        public Category category { get; set; }
    }
}

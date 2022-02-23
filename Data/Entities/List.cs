using System.Collections.Generic;

namespace Inventory_API.Data.Entities
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}

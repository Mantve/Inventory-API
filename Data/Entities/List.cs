using System.Collections.Generic;

namespace Inventory_API.Data.Entities
{
    public class List //shopping list, maintenance list, etc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}

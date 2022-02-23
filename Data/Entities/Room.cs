using System.Collections.Generic;

namespace Inventory_API.Data.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> SharedWith { get; set; }
        public User Author { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Entities
{
    public class ListItem
    {
        public int Id { get; set; }
       public Item Item { get; set; }
        public bool Completed { get; set; }
        public List ParentList { get; set; }
    }
}

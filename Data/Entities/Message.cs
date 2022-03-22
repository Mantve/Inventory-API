using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public User Recipient { get; set; }
        public User Author { get; set; }
        public string Contents { get; set; }
        public MessageType MessageType { get; set; }
    }
}

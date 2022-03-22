using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Entities
{
    public class Reminder
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public string Reason { get; set; }
        public DateTime ReminderTime { get; set; }
        public RepeatFrequency RepeatFrequency { get; set; }
        public User Author { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Entities
{
    public class Reminder
    {
        public int Id { get; set; }
        [Required] public Item Item { get; set; }
        [MaxLength(64)] public string Reason { get; set; }
        [Required] public DateTime ReminderTime { get; set; }
        [DefaultValue(RepeatFrequency.None)] public RepeatFrequency RepeatFrequency { get; set; }
        [Required] public User Author { get; set; }
    }

}

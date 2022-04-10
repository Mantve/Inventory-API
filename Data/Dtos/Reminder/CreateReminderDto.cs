using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Reminder
{
    public record CreateReminderDto(
        [Required()] int? ItemId,
        string Reason,
        [Required()] DateTime ReminderTime,
        int RepeatFrequency);
}

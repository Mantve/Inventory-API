using Inventory_API.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Reminder

{
    public record UpdateReminderDto(
        string Reason,
        DateTime ReminderTime,
        RepeatFrequency RepeatFrequency);
}

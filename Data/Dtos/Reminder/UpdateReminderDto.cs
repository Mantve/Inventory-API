using System;

namespace Inventory_API.Data.Dtos.Reminder

{
    public record UpdateReminderDto(
        string Reason,
        DateTime ReminderTime,
        int RepeatFrequency);
}

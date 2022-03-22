using Inventory_API.Data.Dtos.Item;
using Inventory_API.Data.Entities;
using System;

namespace Inventory_API.Data.Dtos.Reminder
{
    public record ReminderDto(
        int Id,
        ItemDto Item,
        string Reason,
        DateTime ReminderTime,
        RepeatFrequency RepeatFrequency);
}

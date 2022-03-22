using Inventory_API.Data.Dtos.Item;
using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;
using System;

namespace Inventory_API.Data.Dtos.Message
{
    public record MessageDto(
        int Id,
        UserDto Recipient,
        UserDto Author,
        string Contents,
        MessageType MessageType);
}

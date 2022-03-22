using Inventory_API.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Message
{
    public record CreateMessageDto(
        [Required()] int? RecipientId,
        [Required()] int? AuthorId,
        string Contents,
        [Required()] MessageType MessageType);
}

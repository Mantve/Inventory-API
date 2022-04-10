using Inventory_API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Message
{
    public record CreateMessageDto(
        [Required()] string RecipientName,
        string Contents,
        [Required()] MessageType MessageType);
}

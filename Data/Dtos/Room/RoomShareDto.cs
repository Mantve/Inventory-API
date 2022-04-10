using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Room
{
    public record RoomShareDto(
       [Required] string Username,
       [Required] bool Shared);
}

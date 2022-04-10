using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Room
{
    public record CreateRoomDto(
        [Required] string Name);

}

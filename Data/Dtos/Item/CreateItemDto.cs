using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Item
{
    public record CreateItemDto(
        [Required] string Name,
        [Required] float Quantity,
        decimal Value,
        int CategoryId, 
        int? ParentItemId,
        string Comments, 
        [Required] int RoomId);

}

using Inventory_API.Data.Dtos.Category;

namespace Inventory_API.Data.Dtos.Item
{
    public record UpdateItemDto( string Name, float Quantity, decimal Value, CategoryDto Category, int? ParentItemId, string Comments, int RoomId);

}

using Inventory_API.Data.Dtos.Category;
using Inventory_API.Data.Dtos.Room;

namespace Inventory_API.Data.Dtos.Item
{
    public record ChildlessItemDto(
        int Id,
        string Name,
        float Quantity,
        decimal Value,
        CategoryDto Category,
        string Comments,
        RoomDto Room,
        int Level,
        ChildlessItemDto ParentItem,
        bool Sold);
}

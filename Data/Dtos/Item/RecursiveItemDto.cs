using Inventory_API.Data.Dtos.Category;
using Inventory_API.Data.Dtos.Room;
using System.Collections.Generic;

namespace Inventory_API.Data.Dtos.Item
{
    public record RecursiveItemDto(
        int Id,
        string Name, 
        float Quantity,
        decimal Value,
        string Comments,
        RoomDto Room,
        CategoryDto Category, 
        List<RecursiveItemDto> Items);

}

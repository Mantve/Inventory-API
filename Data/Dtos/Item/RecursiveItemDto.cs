using Inventory_API.Data.Dtos.Category;
using System.Collections.Generic;

namespace Inventory_API.Data.Dtos.Item
{
    public record RecursiveItemDto(int Id, string Name, float Quantity, decimal Value, CategoryDto Category, List<RecursiveItemDto> Items);

}

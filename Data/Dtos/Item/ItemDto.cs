using Inventory_API.Data.Dtos.Category;

namespace Inventory_API.Data.Dtos.Item
{
    public record ItemDto(int Id, string Name, float Quantity, decimal Value, CategoryDto Category);
}

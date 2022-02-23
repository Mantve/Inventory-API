using Inventory_API.Data.Dtos.Item;
using System.Collections.Generic;

namespace Inventory_API.Data.Dtos.List
{
    public record ListDto(int Id, string Name, List<ItemDto> Items);
}

using Inventory_API.Data.Dtos.ListItem;
using System.Collections.Generic;

namespace Inventory_API.Data.Dtos.List
{
    public record ListDto(int Id, string Name, List<ListItemDto> Items, int ItemCount);
}

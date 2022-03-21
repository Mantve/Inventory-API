using Inventory_API.Data.Dtos.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Dtos.ListItem
{
    public record ListItemDto(int Id, ItemDto Item, int ParentListId, bool Completed);

}

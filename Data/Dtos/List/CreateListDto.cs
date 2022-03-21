using Inventory_API.Data.Dtos.Item;
using Inventory_API.Data.Dtos.ListItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.List
{
    public record CreateListDto([Required()]string Name);
}

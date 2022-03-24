
using Inventory_API.Data.Dtos.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Dtos.ListItem
{
    public record CreateListItemDto([Required()] int? ItemId, [Required()] int? ParentListId, bool Completed = false );

}

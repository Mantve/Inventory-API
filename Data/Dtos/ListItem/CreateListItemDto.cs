using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.ListItem
{
    public record CreateListItemDto([Required()] int? ItemId, [Required()] int? ParentListId, bool Completed = false);

}

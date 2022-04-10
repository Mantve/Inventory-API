using Inventory_API.Data.Dtos.Item;
using Inventory_API.Data.Dtos.List;

namespace Inventory_API.Data.Dtos.ListItem
{
    public record ListItemDto(
        int Id,
        ItemDto Item,
        BareListDto ParentList,
        bool Completed);

}

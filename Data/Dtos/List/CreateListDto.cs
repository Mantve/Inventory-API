using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.List
{
    public record CreateListDto([Required()] string Name);
}

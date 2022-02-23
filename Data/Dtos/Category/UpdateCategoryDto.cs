using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Category
{
    public record UpdateCategoryDto([Required] string Name);

}

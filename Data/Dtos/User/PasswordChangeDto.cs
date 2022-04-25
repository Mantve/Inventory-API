using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.User
{
    public record PasswordChangeDto([Required()] string OldPassword, [Required()] string NewPassword);

}

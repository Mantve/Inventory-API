using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.User
{
    public record RegisterDto([Required()] string Email, [Required()] string Username, [Required()] string Password);

}

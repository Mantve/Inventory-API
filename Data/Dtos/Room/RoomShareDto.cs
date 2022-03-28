using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Dtos.Room
{
    public record RoomShareDto(
       [Required] string Username,
       [Required] bool Shared);
}

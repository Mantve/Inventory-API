using Inventory_API.Data.Dtos.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Dtos.Room
{
    public record CreateRoomDto([Required] string Name, List<UserDto> SharedWith);

}

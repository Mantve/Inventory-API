using Inventory_API.Data.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Dtos.Room
{
    public record UpdateRoomDto(string Name, List<UserDto> SharedWith);

}

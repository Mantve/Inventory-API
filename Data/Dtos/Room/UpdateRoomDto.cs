using Inventory_API.Data.Dtos.User;
using System.Collections.Generic;

namespace Inventory_API.Data.Dtos.Room
{
    public record UpdateRoomDto(string Name, List<UserDto> SharedWith);

}

using Inventory_API.Data.Dtos.User;
using System.Collections.Generic;

namespace Inventory_API.Data.Dtos.Room
{
    public record RoomDto(int Id, string Name, List<UserDto> SharedWith, UserDto Author);

}

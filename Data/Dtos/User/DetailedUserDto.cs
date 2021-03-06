using Inventory_API.Data.Dtos.List;
using Inventory_API.Data.Dtos.Room;
using System.Collections.Generic;

namespace Inventory_API.Data.Dtos.User
{
    public record DetailedUserDto(string Email, string Username, string Role, List<UserDto> Friends, List<RoomDto> CreatedRooms, List<ListDto> Lists);

}

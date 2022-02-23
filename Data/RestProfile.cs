using AutoMapper;
using Inventory_API.Data.Dtos.List;
using Inventory_API.Data.Dtos.Room;
using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data
{
    public class RestProfile : Profile
    {
        public RestProfile()
        {
            this.CreateMap<User, UserDto>();
            this.CreateMap<User, DetailedUserDto>();
            this.CreateMap<CreateRoomDto, Room>();
            this.CreateMap<UpdateRoomDto, Room>();
            this.CreateMap<Room, RoomDto>();
            this.CreateMap<List, ListDto>();
        }
    }
}

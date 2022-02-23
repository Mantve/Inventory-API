using AutoMapper;
using Inventory_API.Data.Dtos.Category;
using Inventory_API.Data.Dtos.Item;
using Inventory_API.Data.Dtos.List;
using Inventory_API.Data.Dtos.Room;
using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;

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
            this.CreateMap<CreateCategoryDto, Category>();
            this.CreateMap<UpdateCategoryDto, Category>();
            this.CreateMap<Category, CategoryDto>();
            this.CreateMap<CategoryDto, Category>();
            this.CreateMap<CreateItemDto, Item>();
            this.CreateMap<UpdateItemDto, Item>();
            this.CreateMap<Item, ItemDto>();
            this.CreateMap<Item, RecursiveItemDto>();
        }
    }
}

using AutoMapper;
using Inventory_API.Data.Dtos.Category;
using Inventory_API.Data.Dtos.Item;
using Inventory_API.Data.Dtos.List;
using Inventory_API.Data.Dtos.ListItem;
using Inventory_API.Data.Dtos.Message;
using Inventory_API.Data.Dtos.Reminder;
using Inventory_API.Data.Dtos.Room;
using Inventory_API.Data.Dtos.Subscription;
using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;

namespace Inventory_API.Data
{
    public class RestProfile : Profile
    {
        public RestProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, DetailedUserDto>();
            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();
            CreateMap<Room, RoomDto>();
            CreateMap<List, ListDto>();
            CreateMap<List, BareListDto>();
            CreateMap<CreateListDto, List>();
            CreateMap<UpdateListDto, List>();
            CreateMap<Message, MessageDto>();
            CreateMap<CreateMessageDto, Message>();
            CreateMap<Reminder, ReminderDto>();
            CreateMap<CreateReminderDto, Reminder>();
            CreateMap<UpdateReminderDto, Reminder>();
            CreateMap<ListItem, ListItemDto>();
            CreateMap<CreateListItemDto, ListItem>();
            CreateMap<UpdateListItemDto, ListItem>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CreateItemDto, Item>();
            CreateMap<UpdateItemDto, Item>();
            CreateMap<Item, ItemDto>();
            CreateMap<Item, RecursiveItemDto>();
            CreateMap<Item, ParentlessItemDto>();
            CreateMap<CreateSubscriptionDto, Subscription>();

        }
    }
}

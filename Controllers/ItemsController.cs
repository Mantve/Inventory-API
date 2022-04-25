using AutoMapper;
using Inventory_API.Data.Dtos.Item;
using Inventory_API.Data.Entities;
using Inventory_API.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory_API.Controllers
{
    [ApiController]
    [Route("api/item")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ItemsController(IItemRepository itemRepository, IRoomRepository roomRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("/api/room/{roomId}/itemsRecursive")]
        public async Task<ActionResult<IEnumerable<RecursiveItemDto>>> GetAllRecursiveRoom(int roomId)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (await _roomRepository.Get(roomId, username) == null)
            {
                return NotFound($"Room with id {roomId} not found");
            }

            return Ok((await _itemRepository.GetAllRecursiveFromRoom(roomId, username)).Select(o => _mapper.Map<RecursiveItemDto>(o)));
        }

        [Authorize]
        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll(string searchTerm)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            return Ok((await _itemRepository.SearchAll(username, searchTerm)).Select(o => _mapper.Map<ItemDto>(o)));
        }

        [Authorize]
        [HttpGet("{itemId}/all")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll(int itemId)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            return Ok((await _itemRepository.GetAll(itemId, username)).Select(o => _mapper.Map<ItemDto>(o)));
        }

        [Authorize]
        [HttpGet("/api/room/{roomId}/items")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllRoom(int roomId)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (await _roomRepository.Get(roomId, username) == null)
            {
                return NotFound($"Room with id {roomId} not found");
            }

            return Ok((await _itemRepository.GetAllFromRoom(roomId, username)).Select(o => _mapper.Map<ItemDto>(o)));
        }

        [Authorize]
        [HttpGet("{id}/recursive")]
        public async Task<ActionResult<RecursiveItemDto>> GetRecursive(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Item item = await _itemRepository.GetRecursive(id, username);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username))
            {
                return NotFound($"Item with id '{id}' not found.");
            }

            return Ok(_mapper.Map<RecursiveItemDto>(item));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Item item = await _itemRepository.Get(id, username);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username))
            {
                return NotFound($"Item with id '{id}' not found.");
            }

            return Ok(_mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            Item parentItem = null;
            if (dto.ParentItemId != null)
            {
                parentItem = await _itemRepository.Get((int)dto.ParentItemId, username);
                if (parentItem == null || !parentItem.Room.SharedWith.Any(x => x.Username == username))
                {
                    return NotFound($"Parent item with id '{dto.ParentItemId}' not found");
                }
            }

            Room room = await _roomRepository.Get(dto.RoomId, username);
            if (room == null)
            {
                return NotFound($"Room with id '{dto.RoomId}' not found.");
            }

            Category category = await _categoryRepository.Get(x => x.Id == dto.CategoryId && x.Author.Username == username);
            if (category == null)
            {
                return NotFound($"Category with id '{dto.CategoryId}' not found.");
            }

            Item item = _mapper.Map<Item>(dto);
            item.Category = category;
            item.Room = room;
            if (parentItem != null)
            {
                item.ParentItem = parentItem;
            }
            item = await _itemRepository.Create(item);

            return Created(string.Format("/api/item/{0}", item.Id), _mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> Put(int id, UpdateItemDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Item item = await _itemRepository.Get(id, username);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username))
            {
                return NotFound($"Item with id '{id}' not found");
            }

            Item parentItem = null;
            if (dto.ParentItemId != null)
            {
                parentItem = await _itemRepository.Get((int)dto.ParentItemId, username);
                if (parentItem == null || !parentItem.Room.SharedWith.Any(x => x.Username == username))
                {
                    return NotFound($"Parent item with id '{dto.ParentItemId}' not found");
                }
            }

            Room room = await _roomRepository.Get(dto.RoomId, username);
            if (room == null)
            {
                return NotFound($"Room with id '{dto.RoomId}' not found.");
            }

            Category category = await _categoryRepository.Get(x => x.Id == dto.CategoryId && x.Author.Username == username);
            if (category == null)
            {
                return NotFound($"Category with id '{dto.CategoryId}' not found");
            }

            _mapper.Map(dto, item);
            item.Category = category;
            item.ParentItem = parentItem;
            item.Room = room;
            if (await CheckForTreeLoops(item, username))
            {
                return ValidationProblem($"Parent item invalid");
            }
            await _itemRepository.Put(item);

            return Ok(_mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Item item = await _itemRepository.Get(id, username);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username))
            {
                return NotFound($"Item with id '{id}' not found");
            }
            foreach (Item child in item.Items)
            {
                await Delete(child);
            }
            await _itemRepository.Delete(item);

            return NoContent();
        }

        private async Task<bool> CheckForTreeLoops(Item item, string username)
        {
            IEnumerable<Item> items = await _itemRepository.GetAll(item.Id, username);
            if (items.Any(x => x.Id == item.ParentItem.Id))
            {
                return true;
            }
            return false;
        }

        private async Task<ActionResult> Delete(Item item)
        {
            foreach (Item child in item.Items)
            {
                await Delete(child);
            }
            await _itemRepository.Delete(item);

            return NoContent();
        }
    }
}


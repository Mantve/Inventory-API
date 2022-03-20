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
        [HttpGet("all/recursive/{roomId}")]
        public async Task<ActionResult<IEnumerable<RecursiveItemDto>>> GetAllRecursive(int roomId)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (await _roomRepository.Get(roomId, username) == null) return Forbid();
            return Ok((await _itemRepository.GetAllRecursive(roomId)).Select(o => _mapper.Map<RecursiveItemDto>(o)));
        }

        [Authorize]
        [HttpGet("all/{roomId}")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll(int roomId)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (await _roomRepository.Get(roomId, username) == null) return Forbid();
            return Ok((await _itemRepository.GetAll(roomId)).Select(o => _mapper.Map<ItemDto>(o)));
        }

        [Authorize]
        [HttpGet("recursive/{id}")]
        public async Task<ActionResult<RecursiveItemDto>> GetRecursive(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username)) return NotFound($"Item with id '{id}' not found.");

            return Ok(_mapper.Map<RecursiveItemDto>(item));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username)) return NotFound($"Item with id '{id}' not found.");
            return Ok(_mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto dto)
        {
            Item item = _mapper.Map<Item>(dto);
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");

            Item parentItem = null;
            if (dto.ParentItemId != null)
            {
                parentItem = await _itemRepository.Get((int)dto.ParentItemId);
                if (parentItem == null || !parentItem.Room.SharedWith.Any(x => x.Username == username)) return NotFound($"Parent item not found");
            }

            Room room = await _roomRepository.Get(dto.RoomId, username);
            if(room==null) return NotFound($"Room with id'{dto.RoomId}' not found.");

            Category category = await _categoryRepository.Get(dto.CategoryId, username);
            item.Category = category;
            item.Author = user;
            item.Room = room;
            item = await _itemRepository.Create(item);
            if (parentItem != null)
            {
                parentItem.Items.Add(item);
                await _itemRepository.Put(parentItem);
            }
            return Created(string.Format("/api/item/{0}", item.Id), _mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> Put(int id, UpdateItemDto dto) // need to do validation to prevent loops
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username))
                return NotFound("Item not found");
            Item parentItem = await _itemRepository.GetParent(item);
            if (parentItem != null && parentItem.Id != dto.ParentItemId)
            {
                parentItem.Items.Remove(item);
                await _itemRepository.Put(parentItem);
            }
            Item newParentItem = null;
            if (dto.ParentItemId != null)
            {
                newParentItem = await _itemRepository.Get((int)dto.ParentItemId);
                if (newParentItem == null || !item.Room.SharedWith.Any(x => x.Username == username)) return NotFound($"Parent item not found");
            }
            _mapper.Map(dto, item);
            await _itemRepository.Put(item);
            if (newParentItem != null)
            {
                newParentItem.Items.Add(item);
                await _itemRepository.Put(newParentItem);
            }
            return Ok(_mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDto>> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id);
            if (item == null || !item.Room.SharedWith.Any(x => x.Username == username))
                return NotFound("Item not found");
            foreach (Item child in item.Items){
                await Delete(child);
            }
            await _itemRepository.Delete(item);
            return NoContent();
        }

        public async Task<ActionResult<ItemDto>> Delete(Item item)
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


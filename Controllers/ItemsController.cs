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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ItemsController(IItemRepository itemRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._itemRepository = itemRepository;
            this._userRepository = userRepository;
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpGet("recursive")]
        public async Task<IEnumerable<RecursiveItemDto>> GetAllRecursive()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            return (await _itemRepository.GetAll(username)).Select(o => _mapper.Map<RecursiveItemDto>(o));
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            return (await _itemRepository.GetAll(username)).Select(o => _mapper.Map<ItemDto>(o));
        }

        [Authorize]
        [HttpGet("recursive/{id}")]
        public async Task<ActionResult<RecursiveItemDto>> GetRecursive(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id, username);
            if (item == null) return NotFound($"Item with id '{id}' not found.");

            return Ok(_mapper.Map<RecursiveItemDto>(item));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id, username);
            if (item == null) return NotFound($"Item with id '{id}' not found.");

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
            Category category = await _categoryRepository.Get(dto.CategoryId,username);
            item.Category = category;
            item.Author = user;
            item = await _itemRepository.Create(item);
            return Created(string.Format("/api/item/{0}", item.Id), _mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> Put(int id, UpdateItemDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id, username);
            if (item == null)
                return NotFound("Item not found");
            _mapper.Map(dto, item);
            await _itemRepository.Put(item);
            return Ok(_mapper.Map<ItemDto>(item));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDto>> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Item item = await _itemRepository.Get(id, username);
            if (item == null)
                return NotFound("Item not found");
            await _itemRepository.Delete(item);
            return NoContent();
        }
    }
}


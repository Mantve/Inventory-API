using AutoMapper;
using Inventory_API.Data.Dtos.Category;
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
    [Route("api/category")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IUserRepository userRepository, IRoomRepository roomRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            return Ok((await _categoryRepository.GetAll(x => x.Author.Username == username)).Select(o => _mapper.Map<CategoryDto>(o)));
        }

        [Authorize]
        [HttpGet("/api/room/{id}/categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllFromRoom(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Room room = await _roomRepository.Get(id, username);
            if (room == null)
            {
                return NotFound($"Room with id {id} not found");
            }

            List<Category> categories = new();

            IEnumerable<Item> items = await _itemRepository.GetAllFromRoom(id, username);
            foreach (Item item in items)
            {
                categories.Add(item.Category);
            }

            return Ok(categories.Distinct().ToList().Select(o => _mapper.Map<CategoryDto>(o)));
        }

        [Authorize]
        [HttpGet("/api/item/{id}/categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllFromItem(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            IEnumerable<Item> items = await _itemRepository.GetAll(id, username);

            List<Category> categories = new();

            foreach (Item item in items)
            {
                categories.Add(item.Category);
            }

            return Ok(categories.Distinct().ToList().Select(o => _mapper.Map<CategoryDto>(o)));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Category category = await _categoryRepository.Get(x => x.Id == id && x.Author.Username == username);
            if (category == null)
            {
                return NotFound($"Category with id '{id}' not found.");
            }

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Post(CreateCategoryDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetFriends(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            Category category = _mapper.Map<Category>(dto);
            category.Author = user;
            category = await _categoryRepository.Create(category);

            return Created(string.Format("/api/category/{0}", category.Id), _mapper.Map<CategoryDto>(category));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Put(int id, UpdateCategoryDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Category category = await _categoryRepository.Get(x => x.Id == id && x.Author.Username == username);
            if (category == null)
            {
                return NotFound($"Category with id '{id}' not found");
            }

            _mapper.Map(dto, category);
            await _categoryRepository.Put(category);

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Category category = await _categoryRepository.Get(x => x.Id == id && x.Author.Username == username);
            if (category == null)
            {
                return NotFound($"Category with id '{id}' not found");
            }

            await _categoryRepository.Delete(category);
            return NoContent();
        }
    }
}

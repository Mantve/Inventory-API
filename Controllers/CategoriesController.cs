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
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IUserRepository userRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            return (await _categoryRepository.GetAll(username)).Select(o => _mapper.Map<CategoryDto>(o));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Category category = await _categoryRepository.Get(id, username);
            if (category == null) return NotFound($"Category with id '{id}' not found.");

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Post(CreateCategoryDto dto)
        {
            Category category = _mapper.Map<Category>(dto);
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");
            category.Author = user;
            category = await _categoryRepository.Create(category);
            return Created(string.Format("/api/category/{0}", category.Id), _mapper.Map<CategoryDto>(category));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Put(int id, UpdateCategoryDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Category category = await _categoryRepository.Get(id, username);
            if (category == null)
                return NotFound("Category not found");
            _mapper.Map(dto, category);
            await _categoryRepository.Put(category);
            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryDto>> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Category category = await _categoryRepository.Get(id, username);
            if (category == null)
                return NotFound("Category not found");
            await _categoryRepository.Delete(category);
            return NoContent();
        }
    }
}

using AutoMapper;
using Inventory_API.Data.Dtos.List;
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
    [Route("api/list")]
    public class ListsController : ControllerBase
    {

        private readonly IListRepository _listRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ListsController(IListRepository listRepository, IUserRepository userRepository, IMapper mapper)
        {
            _listRepository = listRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ListDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            return (await _listRepository.GetAll(username)).Select(o => _mapper.Map<ListDto>(o));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ListDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            List list = await _listRepository.Get(id, username);
            if (list == null)
            {
                return NotFound($"List with id '{id}' not found.");
            }

            return Ok(_mapper.Map<ListDto>(list));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ListDto>> Post(CreateListDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            List list = _mapper.Map<List>(dto);
            list.Author = user;
            list = await _listRepository.Create(list);

            return Created(string.Format("/api/category/{0}", list.Id), _mapper.Map<ListDto>(list));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ListDto>> Put(int id, UpdateListDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            List list = await _listRepository.Get(id, username);
            if (list == null)
            {
                return NotFound("List not found");
            }

            _mapper.Map(dto, list);
            await _listRepository.Put(list);

            return Ok(_mapper.Map<ListDto>(list));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            List list = await _listRepository.Get(id, username);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await _listRepository.Delete(list);

            return NoContent();
        }
    }

}

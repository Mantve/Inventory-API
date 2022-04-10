using AutoMapper;
using Inventory_API.Data.Dtos.ListItem;
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
    [Route("api/listItem")]
    public class ListItemsController : ControllerBase
    {

        private readonly IListItemRepository _listItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IListRepository _listRepository;
        private readonly IMapper _mapper;

        public ListItemsController(IListItemRepository listItemRepository, IListRepository listRepository, IItemRepository itemRepository, IUserRepository userRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _listRepository = listRepository;
            _listItemRepository = listItemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ListItemDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            return (await _listItemRepository.GetAll(username)).Select(o => _mapper.Map<ListItemDto>(o));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ListItemDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            ListItem listItem = await _listItemRepository.Get(id, username);
            if (listItem == null)
            {
                return NotFound($"ListItem with id '{id}' not found.");
            }

            return Ok(_mapper.Map<ListItemDto>(listItem));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ListItemDto>> Post(CreateListItemDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            Item item = await _itemRepository.Get((int)dto.ItemId, username);
            if (item == null)
            {
                return NotFound($"Item with id '{dto.ItemId}' not found.");
            }

            List list = await _listRepository.Get((int)dto.ParentListId, username);
            if (list == null)
            {
                return NotFound($"List with id'{dto.ParentListId}' not found.");
            }

            ListItem listItem = _mapper.Map<ListItem>(dto);
            listItem.ParentList = list;
            listItem.Item = item;
            listItem = await _listItemRepository.Create(listItem);

            list.ItemCount++;
            await _listRepository.Put(list);

            return Created(string.Format("/api/listItem/{0}", listItem.Id), _mapper.Map<ListItemDto>(listItem));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ListItemDto>> Put(int id, UpdateListItemDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            ListItem listItem = await _listItemRepository.Get(id, username);
            if (listItem == null)
            {
                return NotFound($"ListItem with id '{id}' not found");
            }

            _mapper.Map(dto, listItem);
            await _listItemRepository.Put(listItem);

            return Ok(_mapper.Map<ListItemDto>(listItem));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            ListItem listItem = await _listItemRepository.Get(id, username);
            if (listItem == null)
            {
                return NotFound("ListItem not found");
            }


            List list = await _listRepository.Get(listItem.ParentList.Id, username);
            if (list == null)
            {
                return NotFound($"List with id'{listItem.ParentList}' not found.");
            }

            await _listItemRepository.Delete(listItem);

            list.ItemCount--;
            await _listRepository.Put(list);

            return NoContent();
        }
    }
}

using AutoMapper;
using Inventory_API.Data.Dtos.Room;
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
    [Route("api/room")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RoomsController(IRoomRepository roomRepository, IUserRepository userRepository, IMapper mapper)
        {
            this._roomRepository = roomRepository;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<RoomDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            return (await _roomRepository.GetAll(username)).Select(o => _mapper.Map<RoomDto>(o));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Room room = await _roomRepository.Get(id, username);
            if (room == null) return NotFound($"Room with id '{id}' not found.");

            return Ok(_mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(CreateRoomDto dto)
        {
            Room room = _mapper.Map<Room>(dto);
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");
            room.Author = user;
            room.SharedWith.Add(user);
            room = await _roomRepository.Create(room);
            return Created(string.Format("/api/room/{0}", room.Id), _mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomDto>> Put(int id, UpdateRoomDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Room room= await _roomRepository.Get(id,username);
            if (room == null)
                return NotFound("Room not found");
            _mapper.Map(dto, room);
            await _roomRepository.Put(room);
            return Ok(_mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomDto>> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Room room = await _roomRepository.Get(id, username);
            if (room == null)
                return NotFound("Room not found");
            await _roomRepository.Delete(room);
            return NoContent();
        }
    }
}

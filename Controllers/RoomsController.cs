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
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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
            if (room == null)
            {
                return NotFound($"Room with id '{id}' not found.");
            }

            return Ok(_mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(CreateRoomDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            Room room = _mapper.Map<Room>(dto);
            room.Author = user;
            room.SharedWith = new List<User>() { user };
            room = await _roomRepository.Create(room);
            return Created(string.Format("/api/room/{0}", room.Id), _mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomDto>> Put(int id, UpdateRoomDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            Room room = await _roomRepository.Get(id, username);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            _mapper.Map(dto, room);
            await _roomRepository.Put(room);

            return Ok(_mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            Room room = await _roomRepository.Get(id, username);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            await _roomRepository.Delete(room);

            return NoContent();
        }


        [Authorize]
        [HttpPut("{id}/share")]
        public async Task<ActionResult<RoomDto>> Share(int id, RoomShareDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (username == dto.Username)
            {
                return ValidationProblem("You cannot adjust your own permissions");
            }

            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            Room room = await _roomRepository.Get(id, username);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            if (dto.Shared && room.SharedWith.Any(x => x.Username == dto.Username))
            {
                return ValidationProblem("Room is already shared with this user");
            }
            if (!dto.Shared && !room.SharedWith.Any(x => x.Username == dto.Username))
            {
                return ValidationProblem("Room is not shared with this user");
            }

            User newUser = await _userRepository.GetByUsername(dto.Username);
            if (newUser == null)
            {
                return NotFound($"User with username '{dto.Username}' not found.");
            }

            if (dto.Shared)
            {
                room.SharedWith.Add(newUser);
            }
            else
            {
                room.SharedWith.Remove(newUser);
            }
            room = await _roomRepository.Put(room);
            return Created(string.Format("/api/room/{0}", room.Id), _mapper.Map<RoomDto>(room));
        }

    }
}

using AutoMapper;
using Inventory_API.Data.Dtos.Room;
using Inventory_API.Data.Entities;
using Inventory_API.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Inventory_API.Data.Repositories.RoomRepository;

namespace Inventory_API.Controllers
{
    [ApiController]
    [Route("api/room")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository RoomRepository;
        private readonly IUserRepository UserRepository;
        private readonly IMapper Mapper;

        public RoomsController(IRoomRepository roomRepository, IUserRepository userRepository, IMapper mapper)
        {
            this.RoomRepository = roomRepository;
            this.UserRepository = userRepository;
            this.Mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<RoomDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            return (await RoomRepository.GetAll(username)).Select(o => Mapper.Map<RoomDto>(o));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Room room = await RoomRepository.Get(id, username);
            if (room == null) return NotFound($"Room with id '{id}' not found.");

            return Ok(Mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(CreateRoomDto dto)
        {
            Room room = Mapper.Map<Room>(dto);
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await UserRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found."); room.Author = user;
            room = await RoomRepository.Create(room);
            return Created(string.Format("/api/room/{0}", room.Id), Mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomDto>> Put(int id, UpdateRoomDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Room room= await RoomRepository.Get(id,username);
            if (room == null)
                return NotFound("Room not found");
            Mapper.Map(dto, room);
            await RoomRepository.Put(room);
            return Ok(Mapper.Map<RoomDto>(room));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomDto>> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Room room = await RoomRepository.Get(id, username);
            if (room == null)
                return NotFound("Room not found");
            await RoomRepository.Delete(room);
            return NoContent();
        }
    }
}

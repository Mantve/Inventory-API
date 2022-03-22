using AutoMapper;
using Inventory_API.Data.Dtos.Reminder;
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
    [Route("api/reminder")]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RemindersController(IReminderRepository reminderRepository, IUserRepository userRepository, IMapper mapper)
        {
            _reminderRepository = reminderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ReminderDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            return (await _reminderRepository.GetAll(username)).Select(o => _mapper.Map<ReminderDto>(o));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReminderDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Reminder reminder = await _reminderRepository.Get(id, username);
            if (reminder == null) return NotFound($"Reminder with id '{id}' not found.");

            return Ok(_mapper.Map<ReminderDto>(reminder));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReminderDto>> Post(CreateReminderDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");
            Reminder reminder = _mapper.Map<Reminder>(dto);
            reminder.Author = user;
            reminder = await _reminderRepository.Create(reminder);
            return Created(string.Format("/api/reminder/{0}", reminder.Id), _mapper.Map<ReminderDto>(reminder));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ReminderDto>> Put(int id, UpdateReminderDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Reminder reminder = await _reminderRepository.Get(id, username);
            if (reminder == null)
                return NotFound("Reminder not found");
            _mapper.Map(dto, reminder);
            await _reminderRepository.Put(reminder);
            return Ok(_mapper.Map<ReminderDto>(reminder));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReminderDto>> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Reminder reminder = await _reminderRepository.Get(id, username);
            if (reminder == null)
                return NotFound("Reminder not found");
            await _reminderRepository.Delete(reminder);
            return NoContent();
        }
    }
}

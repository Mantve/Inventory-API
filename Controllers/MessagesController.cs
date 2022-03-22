using AutoMapper;
using Inventory_API.Data.Dtos.Message;
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
    [Route("api/message")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessagesController(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<MessageDto>> GetAll()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            return (await _messageRepository.GetAll(username)).Select(o => _mapper.Map<MessageDto>(o));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDto>> Get(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Message message = await _messageRepository.Get(id, username);
            if (message == null) return NotFound($"Message with id '{id}' not found.");

            return Ok(_mapper.Map<MessageDto>(message));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MessageDto>> Post(CreateMessageDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");
            Message message = _mapper.Map<Message>(dto);
            message.Author = user;
            message = await _messageRepository.Create(message);
            return Created(string.Format("/api/message/{0}", message.Id), _mapper.Map<MessageDto>(message));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageDto>> Delete(int id)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            Message message = await _messageRepository.Get(id, username);
            if (message == null)
                return NotFound("Message not found");
            await _messageRepository.Delete(message);
            return NoContent();
        }
    }
}

using AutoMapper;
using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;
using Inventory_API.Data.Repositories;
using Inventory_API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;
        public AuthController(IUserRepository repository, IMessageRepository messageRepository, JwtService jwtService, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = repository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var a = _userRepository.GetByUsername(dto.Username).Result;
            if (a != null)
            {
                return BadRequest(new { message = "Username is taken" });
            }
            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Unverified"
            };

            user = _userRepository.Create(user).Result;
            return Created("success", _mapper.Map<UserDto>(user));
        
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            User user = _userRepository.GetByUsername(dto.Username).Result;

            if (user == null) return BadRequest(new { message = "User not found" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid password" });
            }

            var jwt = _jwtService.Generate( user.Role, user.Username);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });;

            return Ok(_mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> UserInfo()
        {

                string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");

            return Ok(_mapper.Map<UserDto>(user));
        }


        [Authorize]
        [HttpGet("detaileduser")]
        public async Task<ActionResult<DetailedUserDto>> DetailedUserInfo()
        {
                string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");

            return Ok(_mapper.Map<DetailedUserDto>(user));
        }

        [Authorize]
        [HttpGet("friends")]
        public async Task<ActionResult<IEnumerable<DetailedUserDto>>> FriendsInfo()
        {
                string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
                User user = await _userRepository.GetFriends(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");

            return Ok(user.Friends.Select(o => _mapper.Map<DetailedUserDto>(o)));
        } 
        
        [Authorize]
        [HttpPost("friends/{messageId}")]
        public async Task<ActionResult<DetailedUserDto>> AddFriend(int messageId)
        {
                string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
                User user = await _userRepository.GetByUsername(username);
            if (user == null) return NotFound($"User with username '{username}' not found.");
            Message message = await _messageRepository.Get(messageId, username);
            if (message == null) return NotFound($"Message with id '{messageId}' not found.");
            await _messageRepository.Delete(message);
            if (user.Friends != null && user.Friends.Any(x => x.Username == message.Author.Username)) return ValidationProblem("User is already in the friends list");
            if (user.Friends == null)
                user.Friends = new List<User>();
            user.Friends.Add(message.Author);
            await _userRepository.Put(user);
            return Ok(user.Friends.Select(o => _mapper.Map<DetailedUserDto>(o)));
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
    }
}

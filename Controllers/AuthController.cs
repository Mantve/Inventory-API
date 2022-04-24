using AutoMapper;
using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;
using Inventory_API.Data.Repositories;
using Inventory_API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            if (await _userRepository.GetByUsername(dto.Username) != null)
            {
                return BadRequest(new { message = "Username is taken" });
            }
            if (await _userRepository.GetByEmail(dto.Email) != null)
            {
                return BadRequest(new { message = "Email is taken" });
            }
            User user = new()
            {
                Email = dto.Email,
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Unverified"
            };
            user = await _userRepository.Create(user);

            return Created("success", _mapper.Map<UserDto>(user));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            User user = await _userRepository.GetByUsername(dto.Username);
            if (user == null)
            {
                return BadRequest(new { message = "User not found" });
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid password" });
            }

            var jwt = _jwtService.Generate(user.Role, user.Username);
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(_mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetUserInfo()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            return Ok(_mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpPut("user")]
        public async Task<ActionResult<UserDto>> UpdateUserInfo(UpdateUserDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            _mapper.Map(dto, user);
            user = await _userRepository.Put(user);
            return Ok(_mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpPut("changePassword")]
        public async Task<ActionResult<UserDto>> ChangePassword(PasswordChangeDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            var user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            if(!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password))
            {
                return ValidationProblem("Old passwords do not match");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user = await _userRepository.Put(user);

            return Created("success", _mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpGet("detaileduser")]
        public async Task<ActionResult<DetailedUserDto>> DetailedUserInfo()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            return Ok(_mapper.Map<DetailedUserDto>(user));
        }

        [Authorize]
        [HttpGet("friends")]
        public async Task<ActionResult<IEnumerable<DetailedUserDto>>> FriendsInfo()
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            User user = await _userRepository.GetFriends(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            return Ok(user.Friends.Select(o => _mapper.Map<DetailedUserDto>(o)));
        }

        [Authorize]
        [HttpPost("friends/{inviteId}")]
        public async Task<ActionResult<DetailedUserDto>> AcceptInvite(int inviteId)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            Message message = await _messageRepository.Get(inviteId, username);
            if (message == null)
            {
                return NotFound($"Message with id '{inviteId}' not found.");
            }

            User author = await _userRepository.GetByUsername(message.Author.Username);
            if (author == null)
            {
                return NotFound($"User with username '{author}' not found.");
            }

            var res = await AddFriend(user, author);
            if (res is not OkResult)
            {
                return res;
            }
            res = await AddFriend(author, user);
            if (res is not OkResult)
            {
                await RemoveFriend(author, user);
                return res;
            }

            await _messageRepository.Delete(message);

            return Ok(user.Friends.Select(o => _mapper.Map<DetailedUserDto>(o)));
        }


        [Authorize]
        [HttpDelete("friends/{friendUsername}")]
        public async Task<ActionResult<DetailedUserDto>> Unfriend(string friendUsername)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            User user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            User friend = await _userRepository.GetByUsername(friendUsername);
            if (friend == null)
            {
                return NotFound($"User with username '{friend}' not found.");
            }

            var res = await RemoveFriend(user, friend);
            if (res is not OkResult)
            {
                return res;
            }
            res = await RemoveFriend(friend, user);
            if (res is not OkResult)
            {
                await AddFriend(user, friend);
                return res;
            }

            return Ok(user.Friends.Select(o => _mapper.Map<DetailedUserDto>(o)));
        }

        private async Task<ActionResult> AddFriend(User user, User friend)
        {
            if (user.Friends != null && user.Friends.Any(x => x.Username == friend.Username))
            {
                return ValidationProblem("User is already in the friends list");
            }
            if (user.Friends == null)
            {
                user.Friends = new List<User>();
            }
            user.Friends.Add(friend);
            await _userRepository.Put(friend);
            return Ok();
        }

        private async Task<ActionResult> RemoveFriend(User user, User friend)
        {
            User userFriend = null;
            if (user.Friends != null)
            {
                userFriend = user.Friends.FirstOrDefault(x => x.Username == friend.Username);
            }
            if (userFriend == null)
            {
                return ValidationProblem("User is not in the friends list");
            }

            user.Friends.Remove(friend);
            await _userRepository.Put(user);
            return Ok();
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

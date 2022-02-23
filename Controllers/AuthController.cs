using AutoMapper;
using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;
using Inventory_API.Data.Repositories;
using Inventory_API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Inventory_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository repository, JwtService jwtService, IMapper mapper)
        {
            _repository = repository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (_repository.GetByUsername(dto.Username) != null)
            {
                return BadRequest(new { message = "Username is taken" });
            }
            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Unverified"
            };

            return Created("success", _repository.Create(user));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            User user = _repository.GetByUsername(dto.Username).Result;

            if (user == null) return BadRequest(new { message = "User not found" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid password" });
            }

            var jwt = _jwtService.Generate( user.Role, user.Username);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = false
            });

            return Ok(new
            {
                message = "success"
            });
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> UserInfo()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                string username = token.Issuer;

                var user = await _repository.GetByUsername(username);

                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }


        [Authorize]
        [HttpGet("detaileduser")]
        public async Task<ActionResult<DetailedUserDto>> DetailedUserInfo()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                string username = token.Issuer;

                var user = await _repository.GetByUsername(username);

                return Ok(_mapper.Map<DetailedUserDto>(user));
            }
            catch (Exception)
            {
                return Unauthorized();
            }
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

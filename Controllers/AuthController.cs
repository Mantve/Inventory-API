using Inventory_API.Data.Dtos.User;
using Inventory_API.Data.Entities;
using Inventory_API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Inventory_API.Data.Repositories.UsersRepository;

namespace Inventory_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
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
            var user = _repository.GetByUsername(dto.Username);

            if (user == null) return BadRequest(new { message = "User not found" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid password" });
            }

            var jwt = _jwtService.Generate(user.Id, user.Role, user.Username);

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
        public IActionResult UserInfo()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _repository.GetById(userId);

                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        private IActionResult Unauthorized()
        {
            throw new NotImplementedException();
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

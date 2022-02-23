using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Inventory_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ExtrasController : Controller
    {
        public ExtrasController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; private set; }

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(DateTime.Now.ToString());
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [AllowAnonymous]
        [HttpGet("connstr")]
        public IActionResult ConnStr()
        {
            return Ok(Configuration.GetConnectionString("MainDBConnection"));
        }
    }
}

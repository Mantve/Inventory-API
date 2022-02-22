using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}

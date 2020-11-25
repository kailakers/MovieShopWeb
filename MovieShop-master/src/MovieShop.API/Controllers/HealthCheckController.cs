using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok(new { status = "API is Working" });
        }

        [HttpGet]
        [Route("database")]
        public IActionResult GetDataBase()
        {
            return Ok(new { status = "Database is Working" });
        }
    }
}
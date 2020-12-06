using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController : ControllerBase
    {
        private readonly ICastService _castService;

        public CastController(ICastService castService)
        {
            _castService = castService;
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCastById(int id)
        {
            var cast = await _castService.GetCastDetailsById(id);
            if (cast == null)
                return NotFound("Can't find the cast");
            return Ok(cast);
        }
    }
}
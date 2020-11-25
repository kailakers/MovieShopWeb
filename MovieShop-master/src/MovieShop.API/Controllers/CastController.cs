using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetCastDetails(int id)
        {
            var cast = await _castService.GetCastDetailsWithMovies(id);
            return Ok(cast);
        }
    }
}
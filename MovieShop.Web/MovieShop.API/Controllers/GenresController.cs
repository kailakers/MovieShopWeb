using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Services;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllGenre()
        {
            var genres = await _genreService.GetAllGenres();
            if (genres == null)
            {
                return NotFound("Cannot find any genre");
            }

            return Ok(genres);
        }
    }
}
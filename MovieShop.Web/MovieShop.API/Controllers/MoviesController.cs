using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    // Attribute based routing
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase 
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Route("allMovies")]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMovies();
            if (movies == null)
                return NotFound("Cannot find any movie");
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMoviesDetail(int id)
        {
            var movieDetail = await _movieService.GetMovieAsync(id);
            if (movieDetail == null)
                return NotFound("Movie not found");
            return Ok(movieDetail);
        }
        
        [HttpGet]
        [Route("toprevenue")]
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            var movies = await _movieService.GetHighestGrossingMovies();
            
            //not only return the data(mostly json format) but also the http response status
            if (!movies.Any())
                return NotFound("No Movies Found");
            else
            {
                return Ok(movies);
            }
        }

        [HttpGet]
        [Route("genre/{id:int}")]
        public async Task<IActionResult> GetMovieByGenre(int id)
        {
            var movies = await _movieService.GetMoviesByGenre(id);
            if (movies == null)
                return NotFound("No movie found");
            return Ok(movies);
        }

        [HttpGet]
        [Route("toprated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedMovies();
            if (movies == null)
                return NotFound("No movie found");
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetMovieReviewById(int id)
        {
            var movie = await _movieService.GetReviewsForMovie(id);
            if (movie == null)
                return NotFound("No movie found");
            return Ok(movie);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Models;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IPurchaseService _purchaseService;

        public AdminController(IMovieService movieService, IPurchaseService purchaseService)
        {
            _movieService = movieService;
            _purchaseService = purchaseService;
        }
        
        [HttpPost]
        [Route("createMovie")]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest movieCreateRequest)
        {
            if (ModelState.IsValid)
            {
                await _movieService.CreateMovie(movieCreateRequest);
                return Ok();
            }

            return BadRequest(new {Message = "Please correct the information of the movie!"});
        }

        [HttpPut]
        [Route("updateMovie")]
        public async Task<IActionResult> UpdateMovie(MovieCreateRequest movieCreateRequest)
        {
            if (ModelState.IsValid)
            {
                await _movieService.UpdateMovie(movieCreateRequest);
                return Ok();
            }

            return BadRequest(new {Message = "Please correct the information of the movie!"});
        }

        [HttpGet]
        [Route("top")]
        public async Task<IActionResult> GetTopPurchasedMovie()
        {
            if (ModelState.IsValid)
            {
                var movies = await _movieService.GetTopPurchasedMovie();
                return Ok(movies);
            }

            return NotFound("No Movie Found");
        }

        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetWeeklyPurchases()
        {
            if (ModelState.IsValid)
            {
                var purchases = await _purchaseService.GetWeeklyPurchases();
                return Ok(purchases);
            }

            return NotFound("No Purchase record");
        }
    }
}
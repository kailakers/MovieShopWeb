using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using MovieShop.API.Hub;
using MovieShop.Core.Models.Request;
using MovieShop.Core.Models.Response;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        private readonly IHubContext<MovieShopHub> _hubContext;

        public AdminController(IMovieService movieService, IUserService userService, IMemoryCache cache, IHubContext<MovieShopHub> hubContext)
        {
            _movieService = movieService;
            _userService = userService;
            _cache = cache;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("")]
        public ActionResult TriggerNow()
        {
            RecurringJob.Trigger("ChartServiceJob");
            return Ok("job started");
        }

        [HttpPost("movie")]
        public async Task<IActionResult> CreateMovie([FromBody] MovieCreateRequest movieCreateRequest)
        {
            var createdMovie = await _movieService.CreateMovie(movieCreateRequest);
            return CreatedAtRoute("GetMovie", new { id = createdMovie.Id }, createdMovie);
        }

        [HttpPut("movie")]
        public async Task<IActionResult> UpdateMovie([FromBody] MovieCreateRequest movieCreateRequest)
        {
            var createdMovie = await _movieService.UpdateMovie(movieCreateRequest);
            return Ok(createdMovie);
        }

        [HttpGet("purchases")]
        public async Task<IActionResult> GetAllPurchases([FromQuery] int pageSize = 30, [FromQuery] int page = 1)
        {
            var movies = await _movieService.GetAllMoviePurchasesByPagination(pageSize, page);
            return Ok(movies);
        }

        [HttpGet("top")]
        public IActionResult GetTopMovies()
        {
            var movies = _cache.Get<IEnumerable<MovieChartResponseModel>>("chartsData");
            return Ok(movies);
        }

        [HttpGet("push/{data}")]
        public async Task<IActionResult> PushNotification(string data)
        {
            await _hubContext.Clients.All.SendAsync("discountNotification", data);
            return Ok();
        }
    }
}
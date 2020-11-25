using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IMovieService movieService)
        {
            _userService = userService;
            _movieService = movieService;
        }

        [Authorize]
        [HttpPost("purchase")]
        public async Task<ActionResult> CreatePurchase([FromBody] PurchaseRequestModel purchaseRequest)
        {
            await _userService.PurchaseMovie(purchaseRequest);
            return Ok();
        }

        [Authorize]
        [HttpPost("favorite")]
        public async Task<ActionResult> CreateFavorite([FromBody] FavoriteRequestModel favoriteRequest)
        {
            await _userService.AddFavorite(favoriteRequest);
            return Ok();
        }

        [Authorize]
        [HttpPost("unfavorite")]
        public async Task<ActionResult> DeleteFavorite([FromBody] FavoriteRequestModel favoriteRequest)
        {
            await _userService.RemoveFavorite(favoriteRequest);
            return Ok();
        }

        [Authorize]
        [HttpGet("{id:int}/movie/{movieId}/favorite")]
        public async Task<ActionResult> IsFavoriteExists(int id, int movieId)
        {
            var favoriteExists = await _userService.FavoriteExists(id, movieId);
            return Ok(new {isFavorited = favoriteExists});
        }

        [Authorize]
        [HttpPost("review")]
        public async Task<ActionResult> CreateReview([FromBody] ReviewRequestModel reviewRequest)
        {
            await _userService.AddMovieReview(reviewRequest);
            return Ok();
        }

        [Authorize]
        [HttpPut("review")]
        public async Task<ActionResult> UpdateReview([FromBody] ReviewRequestModel reviewRequest)
        {
            await _userService.UpdateMovieReview(reviewRequest);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{userId:int}/movie/{movieId:int}")]
        public async Task<ActionResult> DeleteReview(int userId, int movieId)
        {
            await _userService.DeleteMovieReview(userId, movieId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("{id:int}/purchases")]
        public async Task<ActionResult> GetUserPurchasedMoviesAsync(int id)
        {
            var userMovies = await _userService.GetAllPurchasesForUser(id);
            return Ok(userMovies);
        }

        [Authorize]
        [HttpGet("{id:int}/favorites")]
        public async Task<ActionResult> GetUserFavoriteMoviesAsync(int id)
        {
            var userMovies = await _userService.GetAllFavoritesForUser(id);
            return Ok(userMovies);
        }

        [Authorize]
        [HttpGet("{id:int}/reviews")]
        public async Task<ActionResult> GetUserReviewedMoviesAsync(int id)
        {
            var userMovies = await _userService.GetAllReviewsByUser(id);
            return Ok(userMovies);
        }
    }
}
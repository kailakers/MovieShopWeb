using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Services;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [Authorize]
        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> CreatePurchase(PurchaseRequestModel purchaseRequestModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.PurchaseMovie(purchaseRequestModel);
                return Ok();
            }
            else
            {
                return BadRequest(new {Message="Incorrect purchasing information!"});
            }
        }

        [Authorize]
        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> AddFavorite(FavoriteRequestModel favoriteRequestModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddFavorite(favoriteRequestModel);
                return Ok();
            }

            return BadRequest(new {Message = "Incorrect information"});
        }

        [Authorize]
        [HttpPost]
        [Route("unfavorite")]
        public async Task<IActionResult> RemoveFavorite(FavoriteRequestModel favoriteRequestModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.RemoveFavorite(favoriteRequestModel);
                return Ok();
            }

            return BadRequest(new {Message = "Incorrect information"});
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}/movie/{movieId}/favorite")]
        public async Task<IActionResult> FavoriteExists(int id, int movieId)
        {
            var favoriteExists = await _userService.FavoriteExists(id, movieId);
            return Ok(favoriteExists);
        }

        [Authorize]
        [HttpPost]
        [Route("review")]
        public async Task<IActionResult> CreateReview(ReviewRequestModel reviewRequestModel)
        {
            if (ModelState.IsValid)
            {
                var review = await _userService.AddMovieReview(reviewRequestModel);
                return Ok(review);
            }

            return BadRequest(new {Message = "Incorrect data in Review"});
        }

        [Authorize]
        [HttpPut]
        [Route("review")]
        public async Task<IActionResult> UpdateReview(ReviewRequestModel reviewRequestModel)
        {
            if (ModelState.IsValid)
            {
                var review = await _userService.UpdateMovieReview(reviewRequestModel);
                return Ok(review);
            }

            return BadRequest(new {Message = "Incorrect data in Review"});
        }

        [Authorize]
        [HttpDelete]
        [Route("{userId}/movie/{movieId}")]
        public async Task<IActionResult> DeleteReview(int userId, int movieId)
        {
            if (ModelState.IsValid)
            {
                await _userService.DeleteMovieReview(userId, movieId);
                return Ok();
            }

            return BadRequest("Incorrect data in Review");
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}/purchases")]
        public async Task<IActionResult> GetAllPurchasesForUser(int id)
        {
            var response = await _userService.GetAllPurchasesForUser(id);
            if (response == null)
                return NotFound("Cannot find any purchases");
            return Ok(response);
        }
        
        [Authorize]
        [HttpGet]
        [Route("{id:int}/favorites")]
        public async Task<IActionResult> GetAllFavoritesForUser(int id)
        {
            var response = await _userService.GetAllFavoritesForUser(id);
            if (response == null)
                return NotFound("Cannot find any purchases");
            return Ok(response);
        }
        
        [Authorize]
        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetAllReviewsForUser(int id)
        {
            var response = await _userService.GetAllReviewsForUser(id);
            if (response == null)
                return NotFound("Cannot find any purchases");
            return Ok(response);
        }
    }
}
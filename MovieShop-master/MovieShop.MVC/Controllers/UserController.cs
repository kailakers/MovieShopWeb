using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IMovieService movieService)
        {
            _userService = userService;
            _movieService = movieService;
        }

        //[Authorize]
        [HttpPost("purchase")]
        public async Task<ActionResult> CreatePurchase(PurchaseRequestModel purchaseRequest)
        {
            await _userService.PurchaseMovie(purchaseRequest);
            return Ok();
        }

        [Authorize]
        [HttpPost("favorite")]
        public async Task<ActionResult> CreateFavorite(FavoriteRequestModel favoriteRequest)
        {
            await _userService.AddFavorite(favoriteRequest);
            return Ok();
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Models;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Migrations;

namespace MovieShop.Controllers
{
    public class AdminController:Controller
    {
        private readonly IMovieService _movieService;

        public AdminController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateMovie()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest movieCreateRequest)
        {
            if (!ModelState.IsValid) return View();
            await _movieService.CreateMovie(movieCreateRequest);
            return View();
        }
    }
}
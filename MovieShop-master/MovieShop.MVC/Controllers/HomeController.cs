using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieShop.Core.Exceptions;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.MVC.Infrastructure;
using MovieShop.MVC.Models;

namespace MovieShop.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService _movieService;

        public HomeController(ILogger<HomeController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index method called");
            var topGrossingMovies = await _movieService.GetHighestGrossingMovies();
            return View(topGrossingMovies);
        }

      
        public IActionResult Error()
        {
            var errorDetails = HttpContext.Items["ErrorDetails"];
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
              //  ErrorResponseModel = errorDetails
            });
        }
    }
}

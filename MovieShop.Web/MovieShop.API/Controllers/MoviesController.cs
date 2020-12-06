using Microsoft.AspNetCore.Mvc;

namespace MovieShop.API.Controllers
{
    public class MovieController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
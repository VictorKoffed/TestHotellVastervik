using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Models;

namespace restaurangprojekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return RedirectToAction("Details", "Order", new { id = 1 });
        }

        public IActionResult Bookings()
        {
            return RedirectToAction("Index", "Booking");
        }

        public IActionResult Products()
        {
            return RedirectToAction("Index", "Product");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Models;
using restaurangprojekt.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace restaurangprojekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookingCustomerService _bookingService;

        public HomeController(ILogger<HomeController> logger, BookingCustomerService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        // HÄR laddar vi bord från API:t och skickar till ViewBag
        public async Task<IActionResult> Index()
        {
            ViewBag.Tables = await _bookingService.GetAvailableTablesAsync();
            return View();
        }

        public IActionResult Menu()
        {
            return RedirectToAction("Menu", "Product");
        }

        public IActionResult Drinks()
        {
            return RedirectToAction("Dryckesmeny", "Product");
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

        public IActionResult Dinnertable()
        {
            return RedirectToAction("Index", "Dinnertable");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
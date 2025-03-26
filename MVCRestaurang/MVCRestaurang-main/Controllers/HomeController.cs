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
            
            return Redirect("https://informatik3.ei.hv.se/RestaurangGUI/Product/menu");
        }

        public IActionResult Drinks()
        {
            
            return Redirect("https://informatik3.ei.hv.se/RestaurangGUI/Product/Dryckesmeny");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Orders()
        {
            
            return Redirect("https://informatik3.ei.hv.se/RestaurangGUI/Order/Details/1");
        }

        public IActionResult Bookings()
        {
            
            return Redirect("https://informatik3.ei.hv.se/RestaurangGUI/Booking/Index");
        }

        public IActionResult Products()
        {
            
            return Redirect("https://informatik3.ei.hv.se/RestaurangGUI/Product/Index");
        }

        public IActionResult Dinnertable()
        {
            
            return Redirect("https://informatik3.ei.hv.se/RestaurangGUI/Dinnertable/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
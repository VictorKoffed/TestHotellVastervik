using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Models;
using restaurangprojekt.Services;
using System.Threading.Tasks;

namespace restaurangprojekt.Controllers
{
    public class BookingCustomerController : Controller
    {
        private readonly BookingCustomerService _bookingService;

        public BookingCustomerController(BookingCustomerService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new BookingViewModel
            {
                AvailableTables = await _bookingService.GetAvailableTablesAsync()
            };

            ViewBag.Tables = model.AvailableTables;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tables = await _bookingService.GetAvailableTablesAsync();
                return View(model);
            }

            var success = await _bookingService.CreateBookingAsync(model);

            if (success)
                return RedirectToAction("Index", "Home");

            TempData["Error"] = "Bokningen misslyckades. Försök igen.";
            ViewBag.Tables = await _bookingService.GetAvailableTablesAsync();
            return View(model);
        }


    }
}
using Microsoft.AspNetCore.Mvc;

namespace restaurangprojekt.Controllers
{
    public class BookingCustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

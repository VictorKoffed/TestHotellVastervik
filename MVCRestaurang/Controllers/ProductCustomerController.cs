using Microsoft.AspNetCore.Mvc;

namespace restaurangprojekt.Controllers
{
    public class ProductCustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

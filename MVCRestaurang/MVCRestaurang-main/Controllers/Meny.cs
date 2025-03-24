using Microsoft.AspNetCore.Mvc;

namespace restaurangprojekt.Controllers
{
    public class Meny : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

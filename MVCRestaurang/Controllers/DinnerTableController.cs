using Microsoft.AspNetCore.Mvc;

namespace restaurangprojekt.Controllers
{
    public class DinnerTableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

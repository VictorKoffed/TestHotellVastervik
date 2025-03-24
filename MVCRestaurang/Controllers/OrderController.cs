using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Services;
using restaurangprojekt.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace restaurangprojekt.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // ✅ Skapa en order
        public async Task<IActionResult> Create()
        {
            var success = await _orderService.CreateOrderAsync();
            if (success)
                return RedirectToAction("Index");

            ViewBag.Error = "Kunde inte skapa order.";
            return View();
        }

        // ✅ Hämta en specifik order genom ID
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
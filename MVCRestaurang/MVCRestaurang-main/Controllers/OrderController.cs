using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Services;
using restaurangprojekt.Models;
using System.Threading.Tasks;
using System.Linq;

namespace restaurangprojekt.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Visa endast aktiva ordrar
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var activeOrders = orders?.Where(o => !o.IsCompleted).ToList();
            return View(activeOrders);
        }

        // Visa historik med slutförda ordrar
        public async Task<IActionResult> History()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var completedOrders = orders?.Where(o => o.IsCompleted).ToList();
            return View(completedOrders);
        }

        // Visa detaljerad info om en order
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            var products = await _orderService.GetAllProductsAsync();
            ViewBag.Products = products;

            return View(order);
        }

        // Skapa ny order
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                var order = await _orderService.CreateOrderAsync(orderDto);
                if (order != null)
                    return RedirectToAction(nameof(Details), new { id = order.OrderID });

                ModelState.AddModelError("", "Kunde inte skapa ordern. Kontrollera fälten och försök igen.");
            }
            return View(orderDto);
        }

        // Uppdatera order
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            var orderDto = new CreateOrderDto
            {
                UserID = order.UserID,
                RoomID = order.RoomID,
                IsRoomService = order.IsRoomService,
                LunchQuantity = order.LunchQuantity
            };

            return View(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateOrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _orderService.UpdateOrderAsync(id, orderDto);
                if (success)
                    return RedirectToAction(nameof(Details), new { id });

                ModelState.AddModelError("", "Kunde inte uppdatera ordern. Kontrollera fälten och försök igen.");
            }
            return View(orderDto);
        }

        // Ta bort order (bekräfta)
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _orderService.DeleteOrderAsync(id);
            if (success)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Kunde inte ta bort ordern. Försök igen senare.");
            return RedirectToAction(nameof(Index));
        }

        // Lägg till produkt i order
        [HttpPost]
        public async Task<IActionResult> AddProduct(int orderId, AddProductToOrderDto dto)
        {
            if (dto.ProductID <= 0 || dto.Amount <= 0)
            {
                ModelState.AddModelError("", "Ogiltiga produktdata.");
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            var success = await _orderService.AddProductToOrderAsync(orderId, dto);
            if (!success)
                ModelState.AddModelError("", "Kunde inte lägga till produkten.");

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        // Ta bort produkt från order
        [HttpPost]
        public async Task<IActionResult> RemoveProduct(int orderId, int productId)
        {
            var success = await _orderService.RemoveProductFromOrderAsync(orderId, productId);
            if (!success)
                ModelState.AddModelError("", "Kunde inte ta bort produkten.");

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        // Uppdatera antalet produkter
        [HttpPost]
        public async Task<IActionResult> UpdateProductAmount(int orderId, int productId, int amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Antalet måste vara minst 1.");
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            var success = await _orderService.UpdateProductAmountAsync(orderId, productId, amount);
            if (!success)
                ModelState.AddModelError("", "Kunde inte uppdatera antalet.");

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        // Slutför order
        [HttpPost]
        public async Task<IActionResult> Checkout(int orderId)
        {
            var success = await _orderService.CheckoutOrderAsync(orderId);
            if (!success)
            {
                ModelState.AddModelError("", "Kunde inte slutföra ordern. Försök igen.");
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            TempData["SuccessMessage"] = $"Ordern med ID {orderId} är nu slutförd och finns i historiken.";
            return RedirectToAction(nameof(Index));
        }
    }
}

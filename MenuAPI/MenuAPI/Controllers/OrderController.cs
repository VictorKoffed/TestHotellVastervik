using MenuAPI.Data;
using MenuAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public OrderController(MenuDbContext context)
        {
            _context = context;
        }

        // Skapa en ny order för en användare
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto model)
        {
            var order = new Order
            {
                UserID = model.UserID,
                RoomID = model.RoomID,
                OrderTime = DateTime.UtcNow,
                IsRoomService = model.IsRoomService,
                LunchQuantity = model.LunchQuantity,
                TotalSum = 0,
                OrderProducts = new List<OrderProduct>()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { orderId = order.OrderID }, order);
        }

        // Hämta en order med alla produkter
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }

        // Hämta alla ordrar
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();

            return Ok(orders);
        }

        // Uppdatera en order
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] CreateOrderDto model)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return NotFound("Order not found");

            // Uppdatera egenskaper
            order.UserID = model.UserID;
            order.RoomID = model.RoomID;
            order.IsRoomService = model.IsRoomService;
            order.LunchQuantity = model.LunchQuantity;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // Ta bort en order
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return NotFound("Order not found");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Lägg till en produkt i en order
        [HttpPost("{orderId}/addProduct")]
        public async Task<IActionResult> AddProductToOrder(int orderId, [FromBody] AddProductToOrderDto model)
        {
            var order = await _context.Orders.FindAsync(orderId);
            var product = await _context.Products.FindAsync(model.ProductID);

            if (order == null || product == null)
                return NotFound("Order or Product not found");

            var orderProduct = new OrderProduct
            {
                OrderID = orderId,
                ProductID = model.ProductID,
                Amount = model.Amount
            };

            order.OrderProducts.Add(orderProduct);
            order.TotalSum += (product.Price ?? 0) * model.Amount;

            _context.OrderProducts.Add(orderProduct);
            await _context.SaveChangesAsync();

            return Ok(orderProduct);
        }

        // Ta bort en produkt från en order
        [HttpDelete("{orderId}/removeProduct/{productId}")]
        public async Task<IActionResult> RemoveProductFromOrder(int orderId, int productId)
        {
            var orderProduct = await _context.OrderProducts
                .Include(op => op.Product)
                .FirstOrDefaultAsync(op => op.OrderID == orderId && op.ProductID == productId);

            if (orderProduct == null)
                return NotFound("Product not found in order");

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound("Order not found");

            // Uppdatera totalsumman
            order.TotalSum -= (orderProduct.Product.Price ?? 0) * orderProduct.Amount;

            // Ta bort produkten
            _context.OrderProducts.Remove(orderProduct);
            await _context.SaveChangesAsync();

            return Ok(new { order.TotalSum });
        }

        // Avsluta order (beställa den)
        [HttpPost("{orderId}/checkout")]
        public async Task<IActionResult> CheckoutOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return NotFound("Order not found");

            order.IsCompleted = true; // Markera ordern som slutförd

            await _context.SaveChangesAsync();

            return Ok(new { order.OrderID, order.IsCompleted });
        }

        [HttpPut("{orderId}/updateProductAmount")]
        public async Task<IActionResult> UpdateProductAmount(int orderId, [FromBody] AddProductToOrderDto model)
        {
            // Hämta OrderProduct + Product
            var orderProduct = await _context.OrderProducts
                .Include(op => op.Product)
                .FirstOrDefaultAsync(op => op.OrderID == orderId && op.ProductID == model.ProductID);

            if (orderProduct == null)
                return NotFound("Product not found in order");

            // Kontrollera att priset inte är null
            if (!orderProduct.Product.Price.HasValue)
                return BadRequest("Product price is null.");

            // Hämta ordern
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound("Order not found.");

            // Uppdatera totalsumman:
            order.TotalSum -= (orderProduct.Product.Price.Value * orderProduct.Amount);
            orderProduct.Amount = model.Amount; // nya antalet
            order.TotalSum += (orderProduct.Product.Price.Value * model.Amount);

            await _context.SaveChangesAsync();

            return Ok(orderProduct);
        }

    }

    // DTO-klasser för enklare dataöverföring
    public class CreateOrderDto
    {
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public bool IsRoomService { get; set; }
        public int LunchQuantity { get; set; }
    }

    public class AddProductToOrderDto
    {
        public int ProductID { get; set; }
        public int Amount { get; set; }
    }
}

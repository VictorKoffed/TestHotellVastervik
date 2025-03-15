using BookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly BookingDbContext _context; 

        public BookingController(BookingDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.Include(b => b.DinnerTable).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.Include(b => b.DinnerTable) .FirstOrDefaultAsync(b => b.BookingID == id); 

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpPost] 
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
        _context.Bookings.Add(booking);
          
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingID }, booking);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateBooking(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException) 
            { 
            if (!_context.Bookings.Any(e => e.BookingID == id))
                {
                    return NotFound();
                }
            else
                {
                    throw;
                }
            

            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        /*[HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "API Running" });
        }*/
    }
}

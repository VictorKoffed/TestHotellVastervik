using BookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase  // Ändrat till ControllerBase eftersom vi inte behöver hela Controller-klassen här
    {
        private readonly BookingDbContext _context;

        public BookingController(BookingDbContext context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        [Produces("application/json")] // Explicit ange att svaret ska vara JSON
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            var bookings = await _context.Bookings.Include(b => b.DinnerTable).ToListAsync();

            // Kontrollera om det finns några bokningar
            if (bookings == null || !bookings.Any())
            {
                return NotFound("Inga bokningar hittades.");
            }

            return Ok(bookings); // Returnera data i JSON-format
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        [Produces("application/json")] // Explicit ange att svaret ska vara JSON
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.Include(b => b.DinnerTable)
                                                 .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound("Bokningen hittades inte.");
            }

            return Ok(booking); // Returnera data i JSON-format
        }

        // POST: api/Booking
        [HttpPost]
        [Produces("application/json")] // Explicit ange att svaret ska vara JSON
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            // Kontrollera om bordet finns
            var tableExists = await _context.DinnerTables.AnyAsync(t => t.TableID == booking.TableID_FK);
            if (!tableExists)
            {
                return BadRequest("Det angivna bordet finns inte.");
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Ladda om bokningen med relationen
            var savedBooking = await _context.Bookings
                .Include(b => b.DinnerTable)
                .FirstOrDefaultAsync(b => b.BookingID == booking.BookingID);

            if (savedBooking == null)
            {
                return Problem("Ett oväntat fel uppstod: Bokningen sparades, men kunde inte hämtas.");
            }

            return CreatedAtAction(nameof(GetBooking), new { id = savedBooking.BookingID }, savedBooking);
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        [Produces("application/json")] // Explicit ange att svaret ska vara JSON
        public async Task<IActionResult> UpdateBooking(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return BadRequest("Booking ID i URL matchar inte objektet.");
            }

            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null)
            {
                return NotFound("Bokningen hittades inte.");
            }

            var tableExists = await _context.DinnerTables.AnyAsync(t => t.TableID == booking.TableID_FK);
            if (!tableExists)
            {
                return BadRequest("Det angivna bordet finns inte.");
            }

            _context.Entry(existingBooking).CurrentValues.SetValues(booking);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("En annan process har uppdaterat denna bokning. Försök igen.");
            }

            return NoContent(); // Ingen data att returnera vid en uppdatering
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        [Produces("application/json")] // Explicit ange att svaret ska vara JSON
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound("Bokningen hittades inte.");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent(); // Ingen data att returnera vid en radering
        }
    }
}

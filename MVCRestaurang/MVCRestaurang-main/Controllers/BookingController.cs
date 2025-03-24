using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Services;
using restaurangprojekt.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace restaurangprojekt.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: Booking/Index
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
                return View(booking);

            var createdBooking = await _bookingService.CreateBookingAsync(booking);
            if (createdBooking == null)
            {
                // Hantera fel, t.ex. returnera ett felmeddelande
                ModelState.AddModelError("", "Kunde inte skapa bokning.");
                return View(booking);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (!ModelState.IsValid)
                return View(booking);

            var success = await _bookingService.UpdateBookingAsync(id, booking);
            if (!success)
            {
                // Hantera fel
                ModelState.AddModelError("", "Kunde inte uppdatera bokning.");
                return View(booking);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _bookingService.DeleteBookingAsync(id);
            if (!success)
            {
                // Hantera fel
                return Problem("Kunde inte radera bokning.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
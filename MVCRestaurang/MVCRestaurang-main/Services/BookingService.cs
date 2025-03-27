using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using restaurangprojekt.Models;

namespace restaurangprojekt.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://informatik3.ei.hv.se/BookingAPI/api/Booking";
        private readonly string tablesUrl = "https://informatik3.ei.hv.se/BookingAPI/api/customer/bookings/tables";

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Hämta alla bokningar
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();

            var bookings = await response.Content.ReadFromJsonAsync<IEnumerable<Booking>>();

            // Hämta alla tillgängliga bord
            var tablesResponse = await _httpClient.GetAsync(tablesUrl);
            var availableTables = await tablesResponse.Content.ReadFromJsonAsync<List<DinnerTable>>();

            // Mappa tabellerna till respektive bokning
            foreach (var booking in bookings)
            {
                booking.DinnerTable = availableTables?.FirstOrDefault(t => t.TableID == booking.TableID_FK);
            }

            return bookings;
        }

        // Hämta bokning via ID
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{bookingId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Booking>();
        }

        // Skapa en ny bokning
        public async Task<Booking?> CreateBookingAsync(Booking booking)
        {
            var response = await _httpClient.PostAsJsonAsync(baseUrl, booking);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Booking>();
        }

        // Skapa bokning via BookingViewModel
        public async Task<bool> CreateBookingAsync(BookingViewModel model)
        {
            var bookingData = new
            {
                userID = model.UserID,
                tableID_FK = model.TableID_FK,
                roomID = model.RoomID,
                reservedDate = model.ReservedDate.Date + model.ReservedTime,
                guestCount = model.GuestCount
            };

            var json = JsonConvert.SerializeObject(bookingData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(baseUrl, content);
            return response.IsSuccessStatusCode;
        }

        // Hämta tillgängliga bord
        public async Task<List<DinnerTable>> GetAvailableTablesAsync()
        {
            var response = await _httpClient.GetAsync(tablesUrl);

            if (!response.IsSuccessStatusCode)
                return new List<DinnerTable>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DinnerTable>>(json) ?? new List<DinnerTable>();
        }

        // Uppdatera befintlig bokning
        public async Task<bool> UpdateBookingAsync(int bookingId, Booking booking)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/{bookingId}", booking);
            return response.IsSuccessStatusCode;
        }

        // Ta bort en bokning
        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{bookingId}");
            return response.IsSuccessStatusCode;
        }
    }
}

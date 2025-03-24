using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using restaurangprojekt.Models;

namespace restaurangprojekt.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://informatik3.ei.hv.se/BookingAPI/api/Booking";

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Hämta alla bokningar
        public async Task<IEnumerable<Booking>?> GetAllBookingsAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Booking>>(json);
        }

        // Hämta bokning via ID
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{bookingId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Booking>(json);
        }

        // Skapa en ny bokning
        public async Task<Booking?> CreateBookingAsync(Booking booking)
        {
            // Serialisera objektet till JSON
            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Anropa POST (i ditt API räcker det ofta med /Booking)
            var response = await _httpClient.PostAsync(baseUrl, content);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Booking>(responseJson);
        }

        // Uppdatera befintlig bokning
        public async Task<bool> UpdateBookingAsync(int bookingId, Booking booking)
        {
            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{baseUrl}/{bookingId}", content);
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
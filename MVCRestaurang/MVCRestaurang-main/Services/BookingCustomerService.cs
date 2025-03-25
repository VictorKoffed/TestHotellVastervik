using Newtonsoft.Json;
using restaurangprojekt.Models;
using System.Text;

namespace restaurangprojekt.Services
{
    public class BookingCustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly string bookingUrl = "https://informatik3.ei.hv.se/BookingAPI/api/Booking";
        private readonly string tablesUrl = "https://informatik3.ei.hv.se/BookingAPI/api/customer/bookings/tables";

        public BookingCustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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

            var response = await _httpClient.PostAsync(bookingUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<DinnerTable>> GetAvailableTablesAsync()
        {
            var response = await _httpClient.GetAsync(tablesUrl);

            if (!response.IsSuccessStatusCode)
                return new List<DinnerTable>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DinnerTable>>(json) ?? new List<DinnerTable>();
        }
    }
}
using Newtonsoft.Json;
using restaurangprojekt.Models;
namespace restaurangprojekt.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var response = await _httpClient.GetAsync($"https://informatik3.ei.hv.se/MenuAPI/api/Order/{orderId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(json);
        }

        public async Task<bool> CreateOrderAsync()
        {
            var response = await _httpClient.PostAsync("https://informatik3.ei.hv.se/MenuAPI/api/Order/create", null);
            return response.IsSuccessStatusCode;
        }
    }
}

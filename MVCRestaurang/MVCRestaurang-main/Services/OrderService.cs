using System.Text;
using Newtonsoft.Json;
using restaurangprojekt.Models;

namespace restaurangprojekt.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://informatik3.ei.hv.se/MenuAPI/api/Order";

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{orderId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<Order>(json);

            if (order?.OrderProducts != null)
            {
                var allProducts = await GetAllProductsAsync();

                if (allProducts != null)
                {
                    foreach (var orderProduct in order.OrderProducts)
                    {
                        orderProduct.Product = allProducts.FirstOrDefault(p => p.ProductID == orderProduct.ProductID_FK);
                    }
                }
            }

            return order;
        }

        public async Task<IEnumerable<Order>?> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Order>>(json);
        }

        public async Task<Order?> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var json = JsonConvert.SerializeObject(orderDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/create", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(responseJson);
        }

        public async Task<bool> UpdateOrderAsync(int orderId, CreateOrderDto orderDto)
        {
            var json = JsonConvert.SerializeObject(orderDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{baseUrl}/{orderId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{orderId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddProductToOrderAsync(int orderId, AddProductToOrderDto addProductDto)
        {
            var json = JsonConvert.SerializeObject(addProductDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/{orderId}/addProduct", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveProductFromOrderAsync(int orderId, int productId)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{orderId}/removeProduct/{productId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAmountAsync(int orderId, int productId, int amount)
        {
            var payload = new { ProductID = productId, Amount = amount };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{baseUrl}/{orderId}/updateProductAmount", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckoutOrderAsync(int orderId)
        {
            var response = await _httpClient.PostAsync($"{baseUrl}/{orderId}/checkout", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Product>?> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync("https://informatik3.ei.hv.se/MenuAPI/api/Product");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
        }
    }
}

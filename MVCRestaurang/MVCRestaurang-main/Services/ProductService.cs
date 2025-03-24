using Newtonsoft.Json;
using restaurangprojekt.Models;
using System.Text;
namespace restaurangprojekt.Services

{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://informatik3.ei.hv.se/MenuAPI/api/Product";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>?> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{productId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(json);
        }

        public async Task<Product?> CreateProductAsync(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/create", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(responseJson);
        }

        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{baseUrl}/{productId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{productId}");
            return response.IsSuccessStatusCode;
        }
    }
}
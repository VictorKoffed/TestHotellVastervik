using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using restaurangprojekt.Models;

namespace restaurangprojekt.Services
{
    public class DinnerTableService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://informatik3.ei.hv.se/BookingAPI/api/DinnerTable";

        public DinnerTableService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<DinnerTable>?> GetDinnerTablesAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DinnerTable>>(json);
        }

        public async Task<DinnerTable?> GetDinnerTableByIdAsync(int tableId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{tableId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DinnerTable>(json);
        }

        public async Task<DinnerTable?> CreateTableAsync(DinnerTable table)
        {
            var json = JsonConvert.SerializeObject(table);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(baseUrl, content);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DinnerTable>(responseJson);
        }

        public async Task<bool> UpdateTableAsync(int tableId, DinnerTable table)
        {
            var json = JsonConvert.SerializeObject(table);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{baseUrl}/{tableId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTableAsync(int tableId)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{tableId}");
            return response.IsSuccessStatusCode;
        }
    }
}
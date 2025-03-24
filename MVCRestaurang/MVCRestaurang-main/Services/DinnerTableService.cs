using Newtonsoft.Json;
using restaurangprojekt.Models;
namespace restaurangprojekt.Services
    
{
    public class DinnerTableService
    {
        private readonly HttpClient _httpClient;

        public DinnerTableService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DinnerTable>> GetDinnerTablesAsync()
        {
            var response = await _httpClient.GetStringAsync("https://informatik3.ei.hv.se/BookingAPI/api/DinnerTable");
            return JsonConvert.DeserializeObject<List<DinnerTable>>(response);
        }

        public async Task<DinnerTable> GetDinnerTableByIdAsync(int tableId)
        {
            var response = await _httpClient.GetStringAsync($"https://informatik3.ei.hv.se/BookingAPI/api/DinnerTable/{tableId}");
            return JsonConvert.DeserializeObject<DinnerTable>(response);
        }
    }
}

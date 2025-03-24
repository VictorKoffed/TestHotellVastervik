using Newtonsoft.Json;

namespace restaurangprojekt.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("isVegetarian")]
        public bool IsVegetarian { get; set; }

        [JsonProperty("imageURL")]
        public string ImageURL { get; set; }
    }
}

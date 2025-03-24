using Newtonsoft.Json;

namespace restaurangprojekt.Models
{
    public class OrderProduct
    {
        [JsonProperty("orderID")]
        public int OrderID_FK { get; set; }

        [JsonProperty("productID")]
        public int ProductID_FK { get; set; }

        public int Amount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MenuAPI.Models
{
    public class OrderProduct
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Amount { get; set; }

        // Navigationsproperties
        [JsonIgnore]
        public Order? Order { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}

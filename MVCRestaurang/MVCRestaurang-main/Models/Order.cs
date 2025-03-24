using Newtonsoft.Json;

namespace restaurangprojekt.Models
{
    public class Order
    {
        public int OrderID { get; set; } 
        public int UserID { get; set; } 

        public int RoomID { get; set; } 

        public DateTime OrderTime { get; set; }

        public decimal TotalSum { get; set; } 

        public bool IsRoomService { get; set; } 

        public int LunchQuantity { get; set; }

        [JsonProperty("orderProducts")]
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace BookingAPI.Models
{
    public class DinnerTable
    {
        [Key] 
        public int TableID { get; set; }
        public int Seats { get; set; }
        public string Location { get; set; }

        [JsonIgnore] // Detta ignorerar Bookings när DinnerTable serialiseras
        public ICollection<Booking>? Bookings { get; set; }
    }
}

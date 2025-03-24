using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace restaurangprojekt.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public int UserID { get; set; }

        [ForeignKey("DinnerTable")]
        public int TableID_FK { get; set; }

        public int RoomID { get; set; }
        public DateTime ReservedDate { get; set; }
        public int GuestCount { get; set; }

        public DinnerTable? DinnerTable { get; set; }
    }
}

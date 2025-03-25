using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace restaurangprojekt.Models
{
    public class BookingViewModel
    {
        [Required]
        public int GuestCount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservedDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan ReservedTime { get; set; }

        public string? Requests { get; set; }

        [Required]
        public int TableID_FK { get; set; }

        [Required]
        public int RoomID { get; set; } = 1;

        [Required]
        public int UserID { get; set; } = 1;

        public List<DinnerTable>? AvailableTables { get; set; }
    }
}
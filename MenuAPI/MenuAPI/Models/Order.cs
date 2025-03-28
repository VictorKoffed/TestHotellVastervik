﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MenuAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public int UserID { get; set; } // Från UserAPI
        public int RoomID { get; set; } // Från BookingAPI
        public DateTime OrderTime { get; set; }
        public decimal TotalSum { get; set; }
        public bool IsRoomService { get; set; }
        public int LunchQuantity { get; set; }
        public bool IsCompleted { get; set; } = false;

        // Navigationsproperty
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>(); // 🛠 Fixa null-problemet
    }
}

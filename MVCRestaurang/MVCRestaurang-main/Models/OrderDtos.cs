namespace restaurangprojekt.Models
{
    public class CreateOrderDto
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public bool IsRoomService { get; set; }
        public int LunchQuantity { get; set; }
    }

    public class AddProductToOrderDto
    {
        public int ProductID { get; set; }
        public int Amount { get; set; }
    }
}
namespace restaurangprojekt.Models
{
    public class DinnerTable
    {
        public int TableID { get; set; } 
        public int Seats { get; set; } 

        public string Location { get; set; }

        public List<string> Bookings { get; set; }
    }
}

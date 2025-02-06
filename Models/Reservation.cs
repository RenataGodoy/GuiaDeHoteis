namespace GuiaDeMoteisAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int SuiteId { get; set; }
        public Suite Suite { get; set; } // N:1 relationship with Suite

        public int ClientId { get; set; }
        public Client Client { get; set; } // N:1 relationship with Client

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

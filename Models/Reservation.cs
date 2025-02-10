namespace GuiaDeMoteisAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int SuiteId { get; set; }
        public Suite Suite { get; set; } // N:1 relationship with Suite

        public int ClientId { get; set; }
        public Client Client { get; set; } // N:1 relationship with Client

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

namespace GuiaDeMoteisAPI.Models
{
    public class Suite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int MotelId { get; set; }
        public Motel Motel { get; set; } // N:1 relationship with Motel

        public List<Reservation> Reservations { get; set; } // 1:N relationship with Reservation
    }
}

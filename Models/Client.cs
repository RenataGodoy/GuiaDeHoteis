namespace GuiaDeMoteisAPI.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Senha hash

        public List<Reservation> Reservations { get; set; } // 1:N relationship with Reservation
    }
}

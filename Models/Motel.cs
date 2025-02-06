namespace GuiaDeMoteisAPI.Models
{
    public class Motel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public List<Suite> Suites { get; set; } // 1:N relationship with Suite
    }
}

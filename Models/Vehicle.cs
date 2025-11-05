namespace APIAutoservice156.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty; 
        public string Model { get; set; } = string.Empty; 
        public int Year { get; set; } 
        public string LicensePlate { get; set; } = string.Empty; 
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
    }
}
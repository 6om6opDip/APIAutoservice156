namespace APIAutoservice156.Models.DTO
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = null!;
        public int ClientId { get; set; }
        //ссылка на DTO автора
        public ClientsDTO? Client { get; set; }
    }
}

namespace APIAutoservice156.Models.DTO
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string? VIN { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateVehicleDTO
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string? VIN { get; set; }
        public int ClientId { get; set; }
    }

    public class UpdateVehicleDTO
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public string? LicensePlate { get; set; }
        public string? VIN { get; set; }
    }
}
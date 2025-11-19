namespace APIAutoservice156.Models.DTO
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public string Category { get; set; } = "General";
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateServiceDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public string Category { get; set; } = "General";
    }

    public class UpdateServiceDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? DurationMinutes { get; set; }
        public string? Category { get; set; }
        public bool? IsActive { get; set; }
    }
}

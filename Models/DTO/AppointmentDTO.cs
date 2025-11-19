namespace APIAutoservice156.Models.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public decimal TotalPrice { get; set; }
        public int VehicleId { get; set; }
        public string? VehicleInfo { get; set; }
        public string? ClientName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AppointmentServiceDTO> Services { get; set; } = new List<AppointmentServiceDTO>();
    }

    public class AppointmentServiceDTO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }

    public class CreateAppointmentDTO
    {
        public DateTime AppointmentDateTime { get; set; }
        public string? Notes { get; set; }
        public int VehicleId { get; set; }
        public List<AppointmentServiceItemDTO> Services { get; set; } = new List<AppointmentServiceItemDTO>();
    }

    public class AppointmentServiceItemDTO
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateAppointmentDTO
    {
        public DateTime? AppointmentDateTime { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}
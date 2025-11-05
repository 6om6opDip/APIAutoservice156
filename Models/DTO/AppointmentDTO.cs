namespace APIAutoservice156.Models.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public int VehicleId { get; set; }
        public VehicleDTO? Vehicle { get; set; }
    }
}

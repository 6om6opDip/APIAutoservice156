namespace APIAutoservice156.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = "Scheduled"; // Scheduled, InProgress, Completed, Cancelled
        public string? Notes { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public List<ServiceAppointment> ServiceAppointments { get; set; } = new List<ServiceAppointment>();
    }
}
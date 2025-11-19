namespace APIAutoservice156.Models
{
    public class ServiceAppointment
    {
        public int Id { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public Appointment? Appointment { get; set; }
        public Service? Service { get; set; }
    }
}
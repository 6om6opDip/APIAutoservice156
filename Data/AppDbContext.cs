using Microsoft.EntityFrameworkCore;
using APIAutoservice156.Models;

namespace APIAutoservice156.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ServiceAppointment> ServiceAppointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Остальные настройки...
            modelBuilder.Entity<ServiceAppointment>()
                .HasKey(sa => new { sa.AppointmentId, sa.ServiceId });

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Client)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.ClientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Vehicle)
                .WithMany()
                .HasForeignKey(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceAppointment>()
                .HasOne(sa => sa.Appointment)
                .WithMany(a => a.ServiceAppointments)
                .HasForeignKey(sa => sa.AppointmentId);

            modelBuilder.Entity<ServiceAppointment>()
                .HasOne(sa => sa.Service)
                .WithMany(s => s.ServiceAppointments)
                .HasForeignKey(sa => sa.ServiceId);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
namespace APIAutoservice156.Models
{
    public class APIDBContect : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public APIDBContect(DbContextOptions<APIDBContect> options)
            : base(options) { }
    }
}
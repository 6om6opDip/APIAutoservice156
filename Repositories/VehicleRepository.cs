using Microsoft.EntityFrameworkCore;
using APIAutoservice156.Data;
using APIAutoservice156.Models;

namespace APIAutoservice156.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Client)
                .Include(v => v.Appointments)
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles
                .Include(v => v.Client)
                .Include(v => v.Appointments)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null) return null;

            existingVehicle.Brand = vehicle.Brand;
            existingVehicle.Model = vehicle.Model;
            existingVehicle.Year = vehicle.Year;
            existingVehicle.LicensePlate = vehicle.LicensePlate;
            existingVehicle.VIN = vehicle.VIN;

            await _context.SaveChangesAsync();
            return existingVehicle;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return false;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Vehicle>> GetByClientIdAsync(int clientId)
        {
            return await _context.Vehicles
                .Where(v => v.ClientId == clientId)
                .Include(v => v.Appointments)
                .ToListAsync();
        }
    }
}
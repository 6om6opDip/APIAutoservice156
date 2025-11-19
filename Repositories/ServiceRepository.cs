using Microsoft.EntityFrameworkCore;
using APIAutoservice156.Data;
using APIAutoservice156.Models;

namespace APIAutoservice156.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Services
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<Service> CreateAsync(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Service?> UpdateAsync(int id, Service service)
        {
            var existingService = await _context.Services.FindAsync(id);
            if (existingService == null) return null;

            existingService.Name = service.Name;
            existingService.Description = service.Description;
            existingService.Price = service.Price;
            existingService.DurationMinutes = service.DurationMinutes;
            existingService.Category = service.Category;
            existingService.IsActive = service.IsActive;

            await _context.SaveChangesAsync();
            return existingService;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
            return await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }
    }
}
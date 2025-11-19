using APIAutoservice156.Data;
using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace APIAutoservice156.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly AppDbContext _context;

        public ClientsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.Vehicles)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> CreateAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client?> UpdateAsync(int id, UpdateClientDTO clientDto)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return null;

            if (!string.IsNullOrEmpty(clientDto.FirstName))
                client.FirstName = clientDto.FirstName;

            if (!string.IsNullOrEmpty(clientDto.LastName))
                client.LastName = clientDto.LastName;

            if (clientDto.PhoneNumber != null)
                client.PhoneNumber = clientDto.PhoneNumber;

            if (clientDto.Email != null)
                client.Email = clientDto.Email;

            client.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }
    }
}
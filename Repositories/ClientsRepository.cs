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
            try
            {
                // Временное решение: без Include
                return await _context.Clients
                    //.Include(c => c.Vehicles)  // ЗАКОММЕНТИРОВАТЬ ДЛЯ ТЕСТА
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GetAllAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Clients
                    //.Include(c => c.Vehicles)  // ЗАКОММЕНТИРОВАТЬ ДЛЯ ТЕСТА
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GetByIdAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task<Client> CreateAsync(Client client)
        {
            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ CreateAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task<Client?> UpdateAsync(int id, UpdateClientDTO clientDto)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"❌ UpdateAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null) return false;

                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ DeleteAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }
    }
}
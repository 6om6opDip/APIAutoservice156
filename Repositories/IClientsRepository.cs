using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;

namespace APIAutoservice156.Repositories
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<Client> CreateAsync(Client client);
        Task<Client?> UpdateAsync(int id, UpdateClientDTO clientDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
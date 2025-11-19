using APIAutoservice156.Models;

namespace APIAutoservice156.Repositories
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task<Service> CreateAsync(Service service);
        Task<Service?> UpdateAsync(int id, Service service);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
    }
}
using APIAutoservice156.Models;

namespace APIAutoservice156.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Vehicle>> GetByClientIdAsync(int clientId);
    }
}
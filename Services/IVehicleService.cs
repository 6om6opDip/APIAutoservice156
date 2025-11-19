using APIAutoservice156.Models.DTO;

namespace APIAutoservice156.Services
{
    public interface IVehiclesService
    {
        Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync();
        Task<VehicleDTO?> GetVehicleByIdAsync(int id);
        Task<VehicleDTO> CreateVehicleAsync(CreateVehicleDTO createVehicleDto);
        Task<VehicleDTO?> UpdateVehicleAsync(int id, UpdateVehicleDTO updateVehicleDto);
        Task<bool> DeleteVehicleAsync(int id);
        Task<IEnumerable<VehicleDTO>> GetVehiclesByClientIdAsync(int clientId);
    }
}
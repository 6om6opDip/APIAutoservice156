using APIAutoservice156.Models.DTO;

namespace APIAutoservice156.Services
{
    public interface IServicesService
    {
        Task<IEnumerable<ServiceDTO>> GetAllServicesAsync();
        Task<IEnumerable<ServiceDTO>> GetActiveServicesAsync();
        Task<ServiceDTO?> GetServiceByIdAsync(int id);
        Task<ServiceDTO> CreateServiceAsync(CreateServiceDTO createServiceDto);
        Task<ServiceDTO?> UpdateServiceAsync(int id, UpdateServiceDTO updateServiceDto);
        Task<bool> DeleteServiceAsync(int id);
    }
}
using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;
using APIAutoservice156.Repositories;

namespace APIAutoservice156.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServicesService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<ServiceDTO>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Select(s => new ServiceDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes,
                Category = s.Category,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            });
        }

        public async Task<IEnumerable<ServiceDTO>> GetActiveServicesAsync()
        {
            var services = await _serviceRepository.GetActiveServicesAsync();
            return services.Select(s => new ServiceDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes,
                Category = s.Category,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            });
        }

        public async Task<ServiceDTO?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null) return null;

            return new ServiceDTO
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                DurationMinutes = service.DurationMinutes,
                Category = service.Category,
                IsActive = service.IsActive,
                CreatedAt = service.CreatedAt
            };
        }

        public async Task<ServiceDTO> CreateServiceAsync(CreateServiceDTO createServiceDto)
        {
            var service = new Service
            {
                Name = createServiceDto.Name,
                Description = createServiceDto.Description,
                Price = createServiceDto.Price,
                DurationMinutes = createServiceDto.DurationMinutes,
                Category = createServiceDto.Category,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdService = await _serviceRepository.CreateAsync(service);

            return new ServiceDTO
            {
                Id = createdService.Id,
                Name = createdService.Name,
                Description = createdService.Description,
                Price = createdService.Price,
                DurationMinutes = createdService.DurationMinutes,
                Category = createdService.Category,
                IsActive = createdService.IsActive,
                CreatedAt = createdService.CreatedAt
            };
        }

        public async Task<ServiceDTO?> UpdateServiceAsync(int id, UpdateServiceDTO updateServiceDto)
        {
            var existingService = await _serviceRepository.GetByIdAsync(id);
            if (existingService == null) return null;

            if (!string.IsNullOrEmpty(updateServiceDto.Name))
                existingService.Name = updateServiceDto.Name;

            if (updateServiceDto.Description != null)
                existingService.Description = updateServiceDto.Description;

            if (updateServiceDto.Price.HasValue)
                existingService.Price = updateServiceDto.Price.Value;

            if (updateServiceDto.DurationMinutes.HasValue)
                existingService.DurationMinutes = updateServiceDto.DurationMinutes.Value;

            if (!string.IsNullOrEmpty(updateServiceDto.Category))
                existingService.Category = updateServiceDto.Category;

            if (updateServiceDto.IsActive.HasValue)
                existingService.IsActive = updateServiceDto.IsActive.Value;

            var updatedService = await _serviceRepository.UpdateAsync(id, existingService);
            if (updatedService == null) return null;

            return new ServiceDTO
            {
                Id = updatedService.Id,
                Name = updatedService.Name,
                Description = updatedService.Description,
                Price = updatedService.Price,
                DurationMinutes = updatedService.DurationMinutes,
                Category = updatedService.Category,
                IsActive = updatedService.IsActive,
                CreatedAt = updatedService.CreatedAt
            };
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            return await _serviceRepository.DeleteAsync(id);
        }
    }
}
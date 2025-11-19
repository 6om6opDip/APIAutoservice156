using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;
using APIAutoservice156.Repositories;

namespace APIAutoservice156.Services
{
    public class VehiclesService : IVehiclesService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IClientsRepository _clientsRepository;

        public VehiclesService(IVehicleRepository vehicleRepository, IClientsRepository clientsRepository)
        {
            _vehicleRepository = vehicleRepository;
            _clientsRepository = clientsRepository;
        }

        public async Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Select(v => new VehicleDTO
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                LicensePlate = v.LicensePlate,
                VIN = v.VIN,
                ClientId = v.ClientId,
                ClientName = $"{v.Client?.FirstName} {v.Client?.LastName}",
                CreatedAt = v.CreatedAt
            });
        }

        public async Task<VehicleDTO?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null) return null;

            return new VehicleDTO
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                LicensePlate = vehicle.LicensePlate,
                VIN = vehicle.VIN,
                ClientId = vehicle.ClientId,
                ClientName = $"{vehicle.Client?.FirstName} {vehicle.Client?.LastName}",
                CreatedAt = vehicle.CreatedAt
            };
        }

        public async Task<VehicleDTO> CreateVehicleAsync(CreateVehicleDTO createVehicleDto)
        {
            if (!await _clientsRepository.ExistsAsync(createVehicleDto.ClientId))
            {
                throw new ArgumentException("Client not found");
            }

            var vehicle = new Vehicle
            {
                Brand = createVehicleDto.Brand,
                Model = createVehicleDto.Model,
                Year = createVehicleDto.Year,
                LicensePlate = createVehicleDto.LicensePlate,
                VIN = createVehicleDto.VIN,
                ClientId = createVehicleDto.ClientId,
                CreatedAt = DateTime.UtcNow
            };

            var createdVehicle = await _vehicleRepository.CreateAsync(vehicle);

            return new VehicleDTO
            {
                Id = createdVehicle.Id,
                Brand = createdVehicle.Brand,
                Model = createdVehicle.Model,
                Year = createdVehicle.Year,
                LicensePlate = createdVehicle.LicensePlate,
                VIN = createdVehicle.VIN,
                ClientId = createdVehicle.ClientId,
                CreatedAt = createdVehicle.CreatedAt
            };
        }

        public async Task<VehicleDTO?> UpdateVehicleAsync(int id, UpdateVehicleDTO updateVehicleDto)
        {
            var existingVehicle = await _vehicleRepository.GetByIdAsync(id);
            if (existingVehicle == null) return null;

            if (!string.IsNullOrEmpty(updateVehicleDto.Brand))
                existingVehicle.Brand = updateVehicleDto.Brand;

            if (!string.IsNullOrEmpty(updateVehicleDto.Model))
                existingVehicle.Model = updateVehicleDto.Model;

            if (updateVehicleDto.Year.HasValue)
                existingVehicle.Year = updateVehicleDto.Year.Value;

            if (!string.IsNullOrEmpty(updateVehicleDto.LicensePlate))
                existingVehicle.LicensePlate = updateVehicleDto.LicensePlate;

            if (updateVehicleDto.VIN != null)
                existingVehicle.VIN = updateVehicleDto.VIN;

            var updatedVehicle = await _vehicleRepository.UpdateAsync(id, existingVehicle);
            if (updatedVehicle == null) return null;

            return new VehicleDTO
            {
                Id = updatedVehicle.Id,
                Brand = updatedVehicle.Brand,
                Model = updatedVehicle.Model,
                Year = updatedVehicle.Year,
                LicensePlate = updatedVehicle.LicensePlate,
                VIN = updatedVehicle.VIN,
                ClientId = updatedVehicle.ClientId,
                CreatedAt = updatedVehicle.CreatedAt
            };
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            return await _vehicleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<VehicleDTO>> GetVehiclesByClientIdAsync(int clientId)
        {
            var vehicles = await _vehicleRepository.GetByClientIdAsync(clientId);
            return vehicles.Select(v => new VehicleDTO
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                LicensePlate = v.LicensePlate,
                VIN = v.VIN,
                ClientId = v.ClientId,
                CreatedAt = v.CreatedAt
            });
        }
    }
}
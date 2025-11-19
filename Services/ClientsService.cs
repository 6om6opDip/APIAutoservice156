using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;
using APIAutoservice156.Repositories;

namespace APIAutoservice156.Services
{
    public class ClientsService : IClientsService
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsService(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllClientsAsync()
        {
            var clients = await _clientsRepository.GetAllAsync();
            return clients.Select(c => new ClientDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                CreatedAt = c.CreatedAt,
                VehicleCount = c.Vehicles.Count
            });
        }

        public async Task<ClientDTO?> GetClientByIdAsync(int id)
        {
            var client = await _clientsRepository.GetByIdAsync(id);
            if (client == null) return null;

            return new ClientDTO
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                CreatedAt = client.CreatedAt,
                VehicleCount = client.Vehicles.Count
            };
        }

        public async Task<ClientDTO> CreateClientAsync(CreateClientDTO createClientDto)
        {
            var client = new Client
            {
                FirstName = createClientDto.FirstName,
                LastName = createClientDto.LastName,
                PhoneNumber = createClientDto.PhoneNumber,
                Email = createClientDto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var createdClient = await _clientsRepository.CreateAsync(client);

            return new ClientDTO
            {
                Id = createdClient.Id,
                FirstName = createdClient.FirstName,
                LastName = createdClient.LastName,
                PhoneNumber = createdClient.PhoneNumber,
                Email = createdClient.Email,
                CreatedAt = createdClient.CreatedAt,
                VehicleCount = 0
            };
        }

        public async Task<ClientDTO?> UpdateClientAsync(int id, UpdateClientDTO updateClientDto)
        {
            var client = await _clientsRepository.UpdateAsync(id, updateClientDto);
            if (client == null) return null;

            return new ClientDTO
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                CreatedAt = client.CreatedAt,
                VehicleCount = client.Vehicles.Count
            };
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            return await _clientsRepository.DeleteAsync(id);
        }
    }
}
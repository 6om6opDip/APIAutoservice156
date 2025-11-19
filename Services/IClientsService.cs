using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;

namespace APIAutoservice156.Services
{
    public interface IClientsService
    {
        Task<IEnumerable<ClientDTO>> GetAllClientsAsync();
        Task<ClientDTO?> GetClientByIdAsync(int id);
        Task<ClientDTO> CreateClientAsync(CreateClientDTO createClientDto);
        Task<ClientDTO?> UpdateClientAsync(int id, UpdateClientDTO updateClientDto);
        Task<bool> DeleteClientAsync(int id);
    }
}
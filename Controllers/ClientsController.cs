using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;
using APIAutoservice156.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIAutoservice156.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsController(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
            var clients = await _clientsRepository.GetAllAsync();
            var clientDtos = clients.Select(c => new ClientDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                CreatedAt = c.CreatedAt,
                VehicleCount = c.Vehicles.Count
            });

            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            var client = await _clientsRepository.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var clientDto = new ClientDTO
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                CreatedAt = client.CreatedAt,
                VehicleCount = client.Vehicles.Count
            };

            return Ok(clientDto);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<ClientDTO>> PostClient(CreateClientDTO createClientDto)
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

            var clientDto = new ClientDTO
            {
                Id = createdClient.Id,
                FirstName = createdClient.FirstName,
                LastName = createdClient.LastName,
                PhoneNumber = createdClient.PhoneNumber,
                Email = createdClient.Email,
                CreatedAt = createdClient.CreatedAt,
                VehicleCount = 0
            };

            return CreatedAtAction(nameof(GetClient), new { id = clientDto.Id }, clientDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<IActionResult> PutClient(int id, UpdateClientDTO updateClientDto)
        {
            var client = await _clientsRepository.UpdateAsync(id, updateClientDto);
            if (client == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientsRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
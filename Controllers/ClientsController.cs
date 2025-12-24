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

        [HttpGet("test-no-auth")]
        [AllowAnonymous]
        public async Task<IActionResult> TestNoAuth()
        {
            try
            {
                var clients = await _clientsRepository.GetAllAsync();
                return Ok(new
                {
                    Count = clients.Count(),
                    Message = "✅ Работает без авторизации!",
                    Clients = clients.Select(c => new { c.Id, c.FirstName, c.LastName })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }

        public ClientsController(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        // Тестовый endpoint без авторизации
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok(new
            {
                Message = "✅ API работает!",
                Time = DateTime.UtcNow,
                Status = "OK",
                Repository = _clientsRepository != null ? "Loaded" : "NULL"
            });
        }

        // Основные методы требуют авторизацию
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
            try
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
                    VehicleCount = c.Vehicles?.Count ?? 0
                });

                return Ok(clientDtos);
            }
            catch (Exception ex)
            {
                // Логируем в консоль
                Console.WriteLine($"❌ GetClients error: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            try
            {
                var client = await _clientsRepository.GetByIdAsync(id);
                if (client == null)
                {
                    return NotFound(new { message = "Client not found" });
                }

                var clientDto = new ClientDTO
                {
                    Id = client.Id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    PhoneNumber = client.PhoneNumber,
                    Email = client.Email,
                    CreatedAt = client.CreatedAt,
                    VehicleCount = client.Vehicles?.Count ?? 0
                };

                return Ok(clientDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GetClient error: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<ClientDTO>> PostClient(CreateClientDTO createClientDto)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"❌ PostClient error: {ex.Message}");
                return StatusCode(500, new { error = "Failed to create client" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<IActionResult> PutClient(int id, UpdateClientDTO updateClientDto)
        {
            try
            {
                var client = await _clientsRepository.UpdateAsync(id, updateClientDto);
                if (client == null)
                {
                    return NotFound(new { message = "Client not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ PutClient error: {ex.Message}");
                return StatusCode(500, new { error = "Failed to update client" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var result = await _clientsRepository.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Client not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ DeleteClient error: {ex.Message}");
                return StatusCode(500, new { error = "Failed to delete client" });
            }
        }
    }
}
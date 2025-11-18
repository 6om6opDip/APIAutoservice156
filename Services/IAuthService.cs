using APIAutoservice156.Models;
using APIAutoservice156.Models.DTO;

namespace APIAutoservice156.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterRequestDTO registerDto);
        Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO loginDto);
        string GenerateJwtToken(User user);
    }
}
using APIAutoservice156.Models.DTO;
using APIAutoservice156.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIAutoservice156.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerDto">Данные для регистрации</param>
        /// <returns>Информация о зарегистрированном пользователе</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDto)
        {
            try
            {
                var user = await _authService.RegisterAsync(registerDto);

                if (user == null)
                {
                    return BadRequest(new { error = "User with this email or username already exists" });
                }

                return CreatedAtAction(nameof(Register), new
                {
                    userId = user.Id,
                    message = "User registered successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="loginDto">Данные для входа</param>
        /// <returns>JWT токен и информация о пользователе</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
        {
            try
            {
                var authResponse = await _authService.LoginAsync(loginDto);

                if (authResponse == null)
                {
                    return Unauthorized(new { error = "Invalid credentials" });
                }

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}
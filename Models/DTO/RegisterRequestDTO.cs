namespace APIAutoservice156.Models.DTO
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; } = "User";
    }
}

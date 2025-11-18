namespace APIAutoservice156.Models.DTO
{
    public class ClientsDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public required string PhoneNumber { get; set; }
    }
}

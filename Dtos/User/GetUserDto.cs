using dotnet_simplified_bank.Models;

namespace dotnet_simplified_bank.Dtos.User
{
    public class GetUserDto
    {
        public Guid ID { get; set; }

        public decimal Balance { get; set; }

        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string CpfCnpj { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
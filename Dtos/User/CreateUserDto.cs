using dotnet_simplified_bank.Models;

namespace dotnet_simplified_bank.Dtos
{
    public class CreateUserDto
    {
        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string CpfCnpj { get; set; }

        public UserRole Role { get; set; }
    }
}
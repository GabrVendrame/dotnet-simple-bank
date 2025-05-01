namespace dotnet_simple_bank.Dtos.User
{
    public class CreateUserDto
    {
        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string CpfCnpj { get; set; }

        public required string Phone { get; set; }
    }
}
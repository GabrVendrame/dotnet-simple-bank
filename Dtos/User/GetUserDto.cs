namespace dotnet_simple_bank.Dtos.User
{
    public class GetUserDto
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public string FullName { get; set; }

        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
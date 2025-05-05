namespace dotnet_simple_bank.Dtos.User
{
    public class LoginDto
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
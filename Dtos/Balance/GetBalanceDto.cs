namespace dotnet_simple_bank.Dtos.Balance
{
    public class GetBalanceDto
    {
        public decimal Balance { get; set; }

        public string FullName { get; set; }

        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

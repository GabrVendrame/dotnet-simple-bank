namespace dotnet_simple_bank.Dtos.Transfer
{
    public class CreateTransferResponseDto
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string PayerID { get; set; }
        public string PayeeID { get; set; }
        public string MessageStatus { get; set; } = "Payee notified";
    }
}

namespace dotnet_simple_bank.Dtos
{
    public class RootRoute (int statusCode)
    {
        public string Status { get; set; } = "Alive";
        public string Message { get; set; } = "Welcome to the Simple Bank API!";
        public int Code { get; set; } = statusCode;
    }
}

namespace dotnet_simple_bank.Interfaces
{
    public interface IExternalServices
    {
        Task<bool> AuthTransferAsync();

        Task<bool> MessageTransferReceivedAsync();
    }
}

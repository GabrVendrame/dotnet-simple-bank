namespace dotnet_simplified_bank.Interfaces
{
    public interface IExternalServices
    {
        Task<bool> AuthTransferAsync();

        Task<bool> MessageTransferReceivedAsync();
    }
}

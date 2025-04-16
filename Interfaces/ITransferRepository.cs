using dotnet_simplified_bank.Dtos.Transfer;
using dotnet_simplified_bank.Models;

namespace dotnet_simplified_bank.Interfaces
{
    public interface ITransferRepository
    {
        Task<Transfer> CreateTransferAsync(decimal amount, User payer, User payee);

        Task<Transfer?> GetTransferByIdAsync(string id);

        Task<bool> AddBalanceAsync(User user, decimal balance);
    }
}

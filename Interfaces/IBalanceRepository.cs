using dotnet_simple_bank.Dtos.Balance;
using dotnet_simple_bank.Models;

namespace dotnet_simple_bank.Interfaces
{
    public interface IBalanceRepository
    {
        Task<bool> AddBalanceAsync(User user, decimal balance);

        Task<User?> GetBalance(string email);
    }
}

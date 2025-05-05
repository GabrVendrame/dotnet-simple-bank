using dotnet_simple_bank.Data;
using dotnet_simple_bank.Interfaces;
using dotnet_simple_bank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace dotnet_simple_bank.Repositories
{
    public class BalanceRepository(AppDatabaseContext databaseContext) : IBalanceRepository
    {
        private readonly AppDatabaseContext _databaseContext = databaseContext;

        public async Task<bool> AddBalanceAsync(User user, decimal balance)
        {
            var userExists = await _databaseContext.Users.AnyAsync(u => u.Id == user.Id);
            if (!userExists) return false;

            var transaction = await StartTransactionAsync();

            user.Balance += balance;

            _databaseContext.Users.Update(user);
            var result = await _databaseContext.SaveChangesAsync();

            CommitTransactionAsync(transaction);

            return result >= 1;
        }

        public async Task<User?> GetBalance(string email)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        private async Task<IDbContextTransaction> StartTransactionAsync()
        {
            return await _databaseContext.Database.BeginTransactionAsync();
        }

        private static async void CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

    }
}

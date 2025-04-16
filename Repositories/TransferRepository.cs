using dotnet_simplified_bank.Data;
using dotnet_simplified_bank.Interfaces;
using dotnet_simplified_bank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace dotnet_simplified_bank.Repositories
{
    public class TransferRepository(AppDatabaseContext databaseContext, IExternalServices externalServices) : ITransferRepository
    {
        private readonly AppDatabaseContext _databaseContext = databaseContext;
        private readonly IExternalServices _externalServices = externalServices;

        public async Task<Transfer> CreateTransferAsync(decimal amount, User payer, User payee)
        {
            var transaction = await StartTransactionAsync();

            payer.Balance -= amount;
            payee.Balance += amount;

            _databaseContext.Users.Update(payer);
            _databaseContext.Users.Update(payee);

            var transfer = new Transfer
            {
                Amount = amount,
                PayeeID = payee.Id,
                PayerID = payer.Id
            };

            if (!await _externalServices.AuthTransferAsync())
            {
                RollbackTransactionAsync(transaction);
                return new Transfer { PayeeID = string.Empty, PayerID = string.Empty };
            }

            await _databaseContext.Transfers.AddAsync(transfer);
            await _databaseContext.SaveChangesAsync();

            CommitTransactionAsync(transaction);

            return transfer;
        }

        public async Task<Transfer?> GetTransferByIdAsync(string id)
        {
            var transfer = await _databaseContext.Transfers.FirstOrDefaultAsync(t => t.Id == id);

            return transfer;
        }

        public async Task<bool> AddBalanceAsync(User user, decimal balance)
        {
            var transaction = await StartTransactionAsync();

            user.Balance += balance;

            var result = await _databaseContext.SaveChangesAsync();

            CommitTransactionAsync(transaction);

            return result >= 1;
        }

        private async Task<IDbContextTransaction> StartTransactionAsync()
        {
            return await _databaseContext.Database.BeginTransactionAsync();
        }

        private static async void CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        private static async void RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }
    }
}

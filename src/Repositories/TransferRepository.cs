using dotnet_simple_bank.Data;
using dotnet_simple_bank.Dtos.Transfer;
using dotnet_simple_bank.Interfaces;
using dotnet_simple_bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace dotnet_simple_bank.Repositories
{
    public class TransferRepository(AppDatabaseContext databaseContext, IExternalServices externalServices) : ITransferRepository
    {
        private readonly AppDatabaseContext _databaseContext = databaseContext;
        private readonly IExternalServices _externalServices = externalServices;

        public async Task<Transfer?> CreateTransferAsync(decimal amount, User payer, User payee)
        {
            var transaction = await StartTransactionAsync();

            payer.Balance -= amount;
            payee.Balance += amount;

            _databaseContext.Users.Update(payer);
            _databaseContext.Users.Update(payee);

            if (!await _externalServices.AuthTransferAsync())
            {
                await RollbackTransactionAsync(transaction);
                
                return null;
            }

            var transfer = new Transfer
            {
                Amount = amount,
                PayeeID = payee.Id,
                PayerID = payer.Id
            };

            await _databaseContext.Transfers.AddAsync(transfer);
            await _databaseContext.SaveChangesAsync();

            await CommitTransactionAsync(transaction);

            return transfer;
        }

        public async Task<Transfer?> GetTransferByIdAsync(string id)
        {
            var transfer = await _databaseContext.Transfers.FirstOrDefaultAsync(t => t.Id == id);

            return transfer;
        }

        private async Task<IDbContextTransaction> StartTransactionAsync()
        {
            return await _databaseContext.Database.BeginTransactionAsync();
        }

        private static async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        private static async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }
    }
}

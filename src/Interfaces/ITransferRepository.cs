﻿using dotnet_simple_bank.Models;

namespace dotnet_simple_bank.Interfaces
{
    public interface ITransferRepository
    {
        Task<Transfer?> CreateTransferAsync(decimal amount, User payer, User payee);

        Task<Transfer?> GetTransferByIdAsync(string id);
    }
}

using dotnet_simplified_bank.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_simplified_bank.Data
{
    public class AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        public DbSet<Transfer> Transfers { get; set; }
    }
}
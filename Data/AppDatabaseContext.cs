using dotnet_simplified_bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnet_simplified_bank.Data
{
    public class AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Transfer>().HasOne(t => t.Payer).WithMany().HasForeignKey(t => t.PayerID).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transfer>().HasOne(t => t.Payee).WithMany().HasForeignKey(t => t.PayeeID).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>().HasIndex(u => u.CpfCnpj).IsUnique();

            List<IdentityRole> roles = [
                    new() { Id = "5ab9c640-b672-468f-80a7-4a6f0a125c0e", Name = "User", NormalizedName = "USER" },
                    new() { Id = "e196f8ae-d648-45ad-848b-cb24ea536cd8", Name = "Seller", NormalizedName = "SELLER" },
            ];
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
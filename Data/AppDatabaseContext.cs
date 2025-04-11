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

            List<IdentityRole> roles = [
                    new() { Id = "userId", Name = "User", NormalizedName = "USER" },
                    new() { Id = "sellerId", Name = "Seller", NormalizedName = "SELLER" },
            ];
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
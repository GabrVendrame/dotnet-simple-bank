using dotnet_simple_bank.Data;
using dotnet_simple_bank.Models;
using dotnet_simple_bank.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace simple_bank_test.Unit
{
    public class BalanceRepositoryTests : IDisposable
    {
        private readonly SqliteConnection connection = new("Data Source=:memory:");
        private readonly AppDatabaseContext dbContext;
        private readonly BalanceRepository balanceRepository;

        public BalanceRepositoryTests()
        {
            dbContext = CreateTestDatabase();
            balanceRepository = new BalanceRepository(dbContext);
            dbContext.Database.EnsureCreated();
            SeedTestDatabase();
        }

        private AppDatabaseContext CreateTestDatabase()
        {
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDatabaseContext>().UseSqlite(connection).Options;

            return new AppDatabaseContext(options);
        }
        private async void SeedTestDatabase()
        {
            await dbContext.Users.AddAsync(testUser);
            await dbContext.SaveChangesAsync();
        }

        private static readonly User testUser = new()
        {
            Id = "userID",
            FullName = "Fulano da Silva",
            Balance = 0,
            CpfCnpj = "12345678910",
            Email = "user@example.com",
            PhoneNumber = "11999999999"
        };

        [Fact]
        public async Task ShouldAddBalance()
        {
            var result = await balanceRepository.AddBalanceAsync(testUser, 1000);

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == "userID");

            Assert.True(result);
            Assert.Equal(1000, user.Balance);
        }

        [Fact]
        public async Task ShouldGetBalance()
        {
            var result = await balanceRepository.GetBalance("user@example.com");

            Assert.NotNull(result);
            Assert.IsType<decimal>(result.Balance);
            Assert.IsType<User>(result);
        }

        [Fact]
        public async Task ShouldNotGetBalance_UserNotExist()
        {
            var result = await balanceRepository.GetBalance("userNotInDatabase@email.com");

            Assert.Null(result);
        }

        public void Dispose()
        {
            dbContext.Dispose();
            connection.Close();
            connection.Dispose();
        }
    }
}

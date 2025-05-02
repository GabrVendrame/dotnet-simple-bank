using dotnet_simple_bank.Data;
using dotnet_simple_bank.Interfaces;
using dotnet_simple_bank.Models;
using dotnet_simple_bank.Repositories;
using FakeItEasy;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace simple_bank_test.Unit
{
    public class TransferRepositoryTests
    {
        private readonly AppDatabaseContext dbContext = CreateTestDatabase();
        private readonly IExternalServices extServices = A.Fake<IExternalServices>();
        private readonly ITransferRepository transferRepository;

        public TransferRepositoryTests()
        {
            transferRepository = new TransferRepository(dbContext, extServices);
            dbContext.Database.EnsureCreated();
            SeedTestDatabase();
        }

        private static AppDatabaseContext CreateTestDatabase()
        {
            var connection = new SqliteConnection(connectionString: "Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDatabaseContext>().UseSqlite(connection).Options;

            return new AppDatabaseContext(options);
        }

        private async void SeedTestDatabase()
        {
            await dbContext.Users.AddRangeAsync(payee, payer);
            await dbContext.Transfers.AddAsync(testTransfer);
            await dbContext.SaveChangesAsync();
        }

        private static readonly User payer = new ()
        {
            Id = "payerID",
            FullName = "Payer da Silva",
            Balance = 1234.00M,
            CpfCnpj = "12345678910",
            Email = "payer@example.com",
            PhoneNumber = "11999999999"
        };

        private static readonly User payee = new()
        {
            Id = "payeeID",
            FullName = "Payee Souza",
            Balance = 5678.00M,
            CpfCnpj = "12345678910123",
            Email = "payee@example.com",
            PhoneNumber = "11999999999"
        };

        private static readonly Transfer testTransfer = new()
        {
            Id = "transferID",
            PayeeID = payee.Id,
            PayerID = payer.Id,
            Amount = 1,
            CreatedAt = DateTime.UtcNow
        };

        [Fact]
        public async Task ShouldCreateTransfer()
        {
            A.CallTo(() => extServices.AuthTransferAsync()).Returns(true);

            var transfer = await transferRepository.CreateTransferAsync(200, payer, payee);

            Assert.NotNull(transfer);
            Assert.IsType<Transfer>(transfer);
        }

        [Fact]
        public async Task ShouldNotCreateTransfer_ExternalAuthFailed()
        {
            A.CallTo(() => extServices.AuthTransferAsync()).Returns(false);

            var transfer = await transferRepository.CreateTransferAsync(200, payer, payee);

            Assert.Null(transfer);
        }

        [Fact]
        public async Task ShouldGetTransfer()
        {
            var transfer = await transferRepository.GetTransferByIdAsync("transferID");

            Assert.NotNull(transfer);
            Assert.IsType<Transfer>(transfer);
        }

        [Fact]
        public async Task ShouldNotGetTransfer_NotExists()
        {
            var transfer = await transferRepository.GetTransferByIdAsync("inexistentTransferID");

            Assert.Null(transfer);
        }
    }
}

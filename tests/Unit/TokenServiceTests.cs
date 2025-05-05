using dotnet_simple_bank.Models;
using dotnet_simple_bank.Services;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using static dotnet_simple_bank.Common.CustomExceptions;

namespace simple_bank_test.Unit
{
    public class TokenServiceTests
    {
        private readonly IConfiguration config = A.Fake<IConfiguration>();

        private readonly string validSecret = "this-must-be-a-valid-512-bits-string-secret-for-testing-jwt-token";
        private readonly string invalidSecret = "this-is-a-invalid-512-bits-string-secret-to-jwt-token";

        private readonly User userTest = new ()
            {
                Id = Guid.NewGuid().ToString(),
                Balance = 1000,
                FullName = "Test User",
                CpfCnpj = "12345678901",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserName = "teste@email.com",
                Email = "teste@email.com",
                PhoneNumber = "55000000000"
            };

        [Fact]
        public void ShouldGetToken()
        {
            A.CallTo(() => config["JWT:Secret"]).Returns(validSecret);

            var tokenService = new TokenService(config);

            var token = tokenService.GetToken(userTest);

            Assert.NotEmpty(token);
            Assert.NotEmpty(userTest.Email);
            Assert.NotEmpty(userTest.CpfCnpj);
            Assert.True(token.Length >= 64);
        }

        [Fact]
        public void ShouldNotGetToken_InvalidSecret()
        {
            A.CallTo(() => config["JWT:Secret"]).Returns(invalidSecret);

            var tokenService = new TokenService(config);

            Assert.Throws<SecretLengthException>(() => tokenService.GetToken(userTest));
        }

        [Fact]
        public void ShouldNotGetToken_EmptyEmail()
        {
            A.CallTo(() => config["JWT:Secret"]).Returns(validSecret);
            var tokenService = new TokenService(config);
            userTest.Email = string.Empty;

            Assert.Throws<NullOrEmptyEmailExcepetion>(() => tokenService.GetToken(userTest));
        }

        [Fact]
        public void ShouldNotGetToken_EmptyCpfCnpj()
        {
            A.CallTo(() => config["JWT:Secret"]).Returns(validSecret);

            var tokenService = new TokenService(config);

            userTest.CpfCnpj = string.Empty;

            Assert.Throws<NullOrEmptyCpfCnpjException>(() => tokenService.GetToken(userTest));
        }
    }
}

using dotnet_simple_bank.Models;

namespace dotnet_simple_bank.Interfaces
{
    public interface ITokenService
    {
        string GetToken(User user);
    }
}

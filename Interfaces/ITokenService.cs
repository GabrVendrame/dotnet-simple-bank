using dotnet_simplified_bank.Models;

namespace dotnet_simplified_bank.Interfaces
{
    public interface ITokenService
    {
        string GetToken(User user);
    }
}

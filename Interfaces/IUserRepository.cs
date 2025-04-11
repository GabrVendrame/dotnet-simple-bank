using dotnet_simplified_bank.Dtos;
using dotnet_simplified_bank.Models;

namespace dotnet_simplified_bank.Interfaces
{
    public interface IUserRepository
    {
        bool CreateUser(User user);

        User? GetUser(string userID);

        void DeleteUser(User user);
    }
}
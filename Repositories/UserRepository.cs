using dotnet_simplified_bank.Data;
using dotnet_simplified_bank.Interfaces;
using dotnet_simplified_bank.Models;

namespace dotnet_simplified_bank.Repositories
{
    public class UserRepository(AppDatabaseContext context) : IUserRepository
    {
        private readonly AppDatabaseContext _context = context;
        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() >= 1;
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public User? GetUser(string userID)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userID);

            if (user == null) return null;

            return user;
        }
    }
}
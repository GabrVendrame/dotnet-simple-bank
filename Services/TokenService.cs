using dotnet_simplified_bank.Interfaces;
using dotnet_simplified_bank.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnet_simplified_bank.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private readonly IConfiguration _config = config;
        private readonly SymmetricSecurityKey _securityKey = new(Encoding.UTF8.GetBytes(config["JWT:LoginKey"]!));

        private string CreateToken(User user)
        {
            if (string.IsNullOrEmpty(user.Email)) throw new ArgumentNullException(user.Email);

            var role = user.CpfCnpj.Length == 11 ? "User" : "Seller";

            var claims = new List<Claim>
            {
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Role, role)
            };

            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512);

            var descriptor = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(descriptor);
        }

        public string GetToken(User user)
        {
            return this.CreateToken(user);
        }
    }
}

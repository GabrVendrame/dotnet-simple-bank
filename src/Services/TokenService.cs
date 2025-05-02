using dotnet_simple_bank.Interfaces;
using dotnet_simple_bank.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static dotnet_simple_bank.Common.CustomExceptions;

namespace dotnet_simple_bank.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private readonly IConfiguration _config = config;
        private readonly SymmetricSecurityKey _securityKey = new(Encoding.UTF8.GetBytes(config["JWT:Secret"]!));

        private string CreateToken(User user)
        {
            if (string.IsNullOrEmpty(user.Email)) throw new NullOrEmptyEmailExcepetion();
            if (string.IsNullOrEmpty(user.CpfCnpj)) throw new NullOrEmptyCpfCnpjException();

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
            try
            {
                return CreateToken(user);
            }
            catch (NullOrEmptyEmailExcepetion ex)
            {
                throw new NullOrEmptyEmailExcepetion(ex.Message, ex);
            }
            catch (NullOrEmptyCpfCnpjException ex)
            {
                throw new NullOrEmptyCpfCnpjException(ex.Message, ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new SecretLengthException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the token.", ex);
            }
        }
    }
}

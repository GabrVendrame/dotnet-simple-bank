using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnet_simplified_bank.Models
{
    public class User : IdentityUser
    {
        [Precision(18, 2)]
        public decimal Balance { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string CpfCnpj { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
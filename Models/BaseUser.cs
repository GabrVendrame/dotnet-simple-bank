using System.ComponentModel.DataAnnotations;

namespace dotnet_simplified_bank.Models
{
    public class BaseUser
    {
        [Required]
        [Key]
        public Guid ID { get; set; }

        public decimal Balance { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        [Key]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
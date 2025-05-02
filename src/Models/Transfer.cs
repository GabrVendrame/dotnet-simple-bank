using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_simple_bank.Models
{
    public class Transfer
    {
        [Required]
        [Key]
        public String Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [Required]
        public required String PayerID { get; set; }

        [Required]
        public required String PayeeID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("PayerID")]
        public User Payer { get; set; }

        [Required]
        [ForeignKey("PayeeID")]
        public User Payee { get; set; }
    }
}
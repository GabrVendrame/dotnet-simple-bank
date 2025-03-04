using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_simplified_bank.Models
{
    public class Transfer
    {
        [Required]
        [Key]
        public Guid ID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public required Guid PayerID { get; set; }

        public Guid PayeeID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("PayerID")]
        public required User Payer { get; set; }

        [Required]
        [ForeignKey("PayeeID")]
        public required User Payee { get; set; }
    }
}
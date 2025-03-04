using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_simplified_bank.Models
{
    public class Transfer
    {
        [Required]
        [Key]
        public Guid ID { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public required Guid PayerID { get; set; }

        public Guid? PayeeUserID { get; set; }

        public Guid? PayeeSellerID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("PayerID")]
        public required User User { get; set; }

        [ForeignKey("PayeeUserID")]
        public User? PayeeUser { get; set; }

        [ForeignKey("PayeeSellerID")]
        public Seller? Seller { get; set; }
    }
}
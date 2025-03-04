using System.ComponentModel.DataAnnotations;

namespace dotnet_simplified_bank.Models
{
    public class Seller : BaseUser
    {
        [Required]
        [Key]
        public required string Cnpj { get; set; }
    }
}
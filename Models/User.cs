using System.ComponentModel.DataAnnotations;

namespace dotnet_simplified_bank.Models
{
    public class User : BaseUser
    {
        [Required]
        public required string Cpf { get; set; }
    }
}
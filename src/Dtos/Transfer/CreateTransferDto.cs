using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnet_simple_bank.Dtos.Transfer
{
    public class CreateTransferDto
    {
        [Required]
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [Required]
        public required String PayeeID { get; set; }
    }
}

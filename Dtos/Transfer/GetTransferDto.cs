using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dotnet_simple_bank.Dtos.Transfer
{
    public class GetTransferDto
    {
        public String Id { get; set; }

        public decimal Amount { get; set; }

        public String PayerID { get; set; }

        public String PayeeID { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

using dotnet_simplified_bank.Dtos.Transfer;
using dotnet_simplified_bank.Interfaces;
using dotnet_simplified_bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_simplified_bank.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class TransferController(ITransferRepository transferRepository, UserManager<User> userManager, IExternalServices externalServices) : ControllerBase
    {
        private readonly ITransferRepository _transferRepository = transferRepository;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IExternalServices _externalServices = externalServices;

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromBody] CreateTransferDto transferDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var payer = await _userManager.FindByIdAsync(transferDto.PayerID);

            if (payer!.Balance < transferDto.Amount)
                return BadRequest(new { Error = "Insufficient balance" });

            var payee = await _userManager.FindByIdAsync(transferDto.PayeeID);

            if (payee == null)
                return BadRequest(new { Error = "Payee not found" });

            var transfer = await _transferRepository.CreateTransferAsync(transferDto.Amount, payer, payee);

            var sendMessageToPayee = await _externalServices.MessageTransferReceivedAsync();

            var messageStatus = "Payee notified";

            // TODO: em caso de erro implementar uma fila pra retry?
            if (!sendMessageToPayee) messageStatus = "Error sending message to payee";

            return Created("New Transfer", new { transfer.Id, transfer.Amount, transfer.PayeeID, messageStatus });
        }
    }
}
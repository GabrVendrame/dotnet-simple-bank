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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTransfer([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transfer = await _transferRepository.GetTransferByIdAsync(id);

            if (transfer == null) return BadRequest("Transfer not found");

            var transferDto = new GetTransferDto
            {
                Id = transfer.Id,
                Amount = transfer.Amount,
                PayerID = transfer.PayeeID,
                PayeeID = transfer.PayeeID,
                CreatedAt = transfer.CreatedAt
            };

            return Ok(transferDto);
        }

        [HttpPut("balance/{id}")]
        [Authorize]
        public async Task<IActionResult> AddBalance([FromRoute] string id, [FromBody] decimal balance)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return BadRequest("User not found");

            var success = await _transferRepository.AddBalanceAsync(user, balance);

            if (!success) return StatusCode(500, "Internal Server Error");

            return Ok(new { NewBalance = user.Balance });
        }
    }
}
﻿using dotnet_simple_bank.Common;
using dotnet_simple_bank.Dtos.Transfer;
using dotnet_simple_bank.Interfaces;
using dotnet_simple_bank.Mappers;
using dotnet_simple_bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_simple_bank.Controllers
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

            var payerEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var payer = await _userManager.FindByEmailAsync(payerEmail);

            if (transferDto.Amount <= 0) return BadRequest(CustomErrors.BadRequest("Transfer amount must be greater than 0"));

            if (payer!.Balance < transferDto.Amount)
                return BadRequest(CustomErrors.BadRequest("Insufficient balance"));

            var payee = await _userManager.FindByIdAsync(transferDto.PayeeID);

            if (payee == null)
                return NotFound(CustomErrors.NotFound("Payee not found"));

            var transfer = await _transferRepository.CreateTransferAsync(transferDto.Amount, payer, payee);

            if (transfer == null) return StatusCode(500, CustomErrors.InternalServerError("Transfer failed"));

            var sendMessageToPayee = await _externalServices.MessageTransferReceivedAsync();

            var createdResponse = Mapper.TransferToCreateTransferResponseDto(transfer);

            // TODO: em caso de erro implementar uma fila pra retry?
            if (!sendMessageToPayee) createdResponse.MessageStatus = "Error sending message to payee";

            return Created("New Transfer", createdResponse);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTransfer([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transfer = await _transferRepository.GetTransferByIdAsync(id);

            if (transfer == null) return NotFound(CustomErrors.NotFound("Transfer not found"));

            var transferDto = Mapper.TransferToGetTransferDto(transfer);

            return Ok(transferDto);
        }
    }
}
using dotnet_simple_bank.Common;
using dotnet_simple_bank.Dtos.Balance;
using dotnet_simple_bank.Interfaces;
using dotnet_simple_bank.Mappers;
using dotnet_simple_bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotnet_simple_bank.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class BalanceController(UserManager<User> userManager, IBalanceRepository balanceRepository) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IBalanceRepository _balanceRepository = balanceRepository;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBalance()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var email = User.FindFirst(ClaimTypes.Email).Value;

            var user = await _balanceRepository.GetBalance(email);

            if (user == null) return NotFound(CustomErrors.NotFound("User not found"));

            var balanceResponse = Mapper.UserToGetBalanceDto(user);

            return Ok(balanceResponse);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AddBalance([FromBody] AddBalanceDto addBalanceDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound(CustomErrors.NotFound("User not found"));

            var success = await _balanceRepository.AddBalanceAsync(user, addBalanceDto.Balance);

            if (!success) return StatusCode(500, "Internal Server Error");

            var balanceResponse = Mapper.UserToGetBalanceDto(user);

            return Ok(balanceResponse);
        }
    }
}

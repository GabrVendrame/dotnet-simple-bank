using dotnet_simplified_bank.Dtos;
using dotnet_simplified_bank.Dtos.User;
using dotnet_simplified_bank.Interfaces;
using dotnet_simplified_bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_simplified_bank.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UserController(UserManager<User> userManager, SignInManager<User> loginManager, ITokenService tokenService) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _loginManager = loginManager;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                FullName = createUserDto.FullName,
                CpfCnpj = createUserDto.CpfCnpj,
                PhoneNumber = createUserDto.Phone,
            };

            var createdUser = await _userManager.CreateAsync(user, createUserDto.Password);
            if (createdUser.Succeeded)
            {
                var userRole = user.CpfCnpj.Length == 11 ? "User" : "Seller";

                var applyRole = await _userManager.AddToRoleAsync(user, userRole);

                return applyRole.Succeeded ? Created() : StatusCode(500, applyRole.Errors);
            }

            return BadRequest(createdUser.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

            if (user == null) return NotFound("User not found");

            var result = await _loginManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);

            if (result.Succeeded)
            {
                var loginResponse = new { Token = _tokenService.GetToken(user) };
                return Ok(loginResponse);
            }

            return Unauthorized("Invalid credentials");
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound("User not found");

            var userDto = new GetUserDto
            {
                Id = user.Id,
                Balance = user.Balance,
                FullName = user.FullName,
                Email = user.Email,
                CpfCnpj = user.CpfCnpj,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };

            return Ok(userDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User, Seller")]
        public async Task<IActionResult> EditUser([FromRoute] string id, [FromBody] EditUserDto editDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) NotFound("User not found");

            if (!string.IsNullOrEmpty(editDto.PhoneNumber)) user.PhoneNumber = editDto.PhoneNumber;

            var phoneUpdateResult = await _userManager.UpdateAsync(user);

            if (!phoneUpdateResult.Succeeded) return BadRequest(phoneUpdateResult.Errors);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) NotFound("User not found");

            await _userManager.DeleteAsync(user);

            return NoContent();
        }
    }
}
using dotnet_simplified_bank.Dtos;
using dotnet_simplified_bank.Dtos.User;
using dotnet_simplified_bank.Interfaces;
using dotnet_simplified_bank.Models;
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
                CpfCnpj = createUserDto.CpfCnpj
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
    }
}
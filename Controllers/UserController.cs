using dotnet_simple_bank.Common;
using dotnet_simple_bank.Dtos.User;
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
    public class UserController(UserManager<User> userManager, SignInManager<User> loginManager, ITokenService tokenService) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _loginManager = loginManager;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = Mapper.CreateUserDtoToUser(createUserDto);

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

            if (user == null) return NotFound(CustomErrors.NotFound("User not found"));

            var result = await _loginManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);

            if (result.Succeeded)
            {
                LoginResponseDto loginResponse = new() { Email = user.Email, Token = _tokenService.GetToken(user) };
                return Ok(loginResponse);
            }

            return Unauthorized(CustomErrors.Unauthorized("Invalid Credentials"));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var email = User.FindFirst(ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound(CustomErrors.NotFound("User not found"));

            var userDto = Mapper.UserToGetUserDto(user);

            return Ok(userDto);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditUser([FromBody] EditUserDto editDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var email = User.FindFirst(ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) NotFound(CustomErrors.NotFound("User not found"));

            if (!string.IsNullOrEmpty(editDto.PhoneNumber)) user.PhoneNumber = editDto.PhoneNumber;

            var phoneUpdateResult = await _userManager.UpdateAsync(user);

            if (!phoneUpdateResult.Succeeded) return BadRequest(phoneUpdateResult.Errors);

            var userResponse = Mapper.UserToGetUserDto(user);

            return Ok(userResponse);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) NotFound(CustomErrors.NotFound("User not found"));

            await _userManager.DeleteAsync(user);

            return NoContent();
        }
    }
}
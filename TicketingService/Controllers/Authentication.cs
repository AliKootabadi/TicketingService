using Microsoft.AspNetCore.Mvc;
using TicketingService.DTOs;
using TicketingService.Interfaces;

namespace TicketingService.Controllers
{
    [Route("auth")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly IUserService _userService;

        public Authentication(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(request.Email, request.Password);
                if (user == null)
                    return Unauthorized("Invalid credentials");

                var token = _userService.GenerateJwtToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred during login.");
            }
        }
    }
}

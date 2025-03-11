using Microsoft.AspNetCore.Mvc;
using QuizApp.Business.Services;
using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user, [FromQuery] string password)
        {
            var result = await _userService.RegisterUserAsync(user, password);
            if (result.Succeeded) return Ok("User registered successfully.");
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            var result = await _userService.LoginAsync(email, password);
            if (result.Succeeded) return Ok("Login successful.");
            return Unauthorized("Invalid credentials.");
        }
    }
}

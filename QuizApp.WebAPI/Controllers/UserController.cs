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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserIdBy(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewUser([FromBody] UserCreateViewModel userCreateViewModel)
        {
            if (userCreateViewModel == null)
            {
                return BadRequest("Invalid user data.");
            }

            await _userService.CreateNewUser(userCreateViewModel);
            return CreatedAtAction(nameof(GetUserById), new { id = userCreateViewModel.Id }, userCreateViewModel);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUserById(Guid id, [FromBody] UserEditViewModel userEditViewModel)
        {
            if (userEditViewModel == null)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _userService.UpdateUserById(id, userEditViewModel);
            if (!result)
            {
                return NotFound("User not found or failed to update.");
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            await _userService.DeleteUserById(id);
            return NoContent();
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid password change data.");
            }

            try
            {
                await _userService.ChangePassword(model);
                return Ok("Password changed successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}

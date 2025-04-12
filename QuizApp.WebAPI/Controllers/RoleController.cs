using Microsoft.AspNetCore.Mvc;
using QuizApp.Business.Services;
using QuizApp.Business.ViewModels;

namespace QuizApp.WebAPI.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET /roles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleById(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        // GET /roles
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }

        // POST /roles
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _roleService.CreateNewRole(model);
            return Ok(success);
        }

        // PUT /roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleEditViewModel model)
        {
            if (!ModelState.IsValid || id != model.Id)
                return BadRequest(ModelState);

            var success = await _roleService.UpdateRoleById(id, model);
            return Ok(success);
        }

        // DELETE /roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var success = await _roleService.DeleteRoleById(id);
            return Ok(success);
        }
    }
}

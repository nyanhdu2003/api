using Microsoft.AspNetCore.Identity;
using QuizApp.WebAPI.Models;

namespace QuizApp.Business.Services;

public interface IRoleService
{
    Task<IdentityResult> CreateRoleAsync(Role role);

    Task<bool> RoleExistsAsync(string roleName);

    Task<IdentityResult> AssignRoleToUserAsync(Guid userId, string roleName);
}

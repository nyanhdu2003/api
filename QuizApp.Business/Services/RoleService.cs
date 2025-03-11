using Microsoft.AspNetCore.Identity;
using QuizApp.Models.Models;
using QuizApp.Data.Repositories;
using QuizApp.WebAPI.Models;

namespace QuizApp.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IRoleRepository _roleRepository;

        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<Role> roleManager, IRoleRepository roleRepository, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(Role role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> AssignRoleToUserAsync(Guid userId, string roleName)
        {
            // Tìm user theo ID
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Kiểm tra role có tồn tại không
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                throw new KeyNotFoundException("Role not found.");
            }

            // Gán role cho user
            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}

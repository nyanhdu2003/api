using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp.Business.Services;
using QuizApp.Business.ViewModels;
using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<RoleViewModel> GetRoleById(Guid id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null) return null;

            return new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive
            };
        }

        public async Task<List<RoleViewModel>> GetAllRoles()
        {
            return await _roleManager.Roles
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }

        public async Task<bool> CreateNewRole(RoleCreateViewModel roleCreateViewModel)
        {
            var newRole = new Role
            {
                Name = roleCreateViewModel.Name,
                Description = roleCreateViewModel.Description ?? string.Empty,
                IsActive = roleCreateViewModel.IsActive
            };

            var result = await _roleManager.CreateAsync(newRole);
            return result.Succeeded;
        }

        public async Task<bool> UpdateRoleById(Guid id, RoleEditViewModel roleEditViewModel)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false;

            role.Name = roleEditViewModel.Name;
            role.Description = roleEditViewModel.Description ?? string.Empty;
            role.IsActive = roleEditViewModel.IsActive;

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
}

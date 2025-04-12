using QuizApp.Business.ViewModels;

namespace QuizApp.Business.Services;

public interface IRoleService
{
    Task<RoleViewModel> GetRoleById(Guid id);

    Task<List<RoleViewModel>> GetAllRoles();

    Task<bool> CreateNewRole(RoleCreateViewModel roleCreateViewModel);

    Task<bool> UpdateRoleById(Guid id, RoleEditViewModel roleEditViewModel);

    Task<bool> DeleteRoleById(Guid id);
}

using QuizApp.Business.ViewModels;

namespace QuizApp.Business.Services;

public interface IUserService
{
    Task<UserViewModel> GetUserIdBy(Guid id);

    Task<List<UserViewModel>> GetAllUsers();

    Task<bool> CreateNewUser(UserCreateViewModel userCreateViewModel);

    Task<bool> UpdateUserById(Guid id, UserEditViewModel userEditViewModel);

    Task<bool> DeleteUserById(Guid id);

    Task<bool> ChangePassword(ChangePasswordViewModel changePasswordViewModel);

}

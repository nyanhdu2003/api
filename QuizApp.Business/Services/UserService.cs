using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp.Business.ViewModels;
using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;

namespace QuizApp.Business.Services;

public class UserService : IUserService
{
    private readonly QuizAppDbContext _context;

    private readonly UserManager<User> _userManager;

    public UserService(QuizAppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

public async Task<bool> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
{
    var user = await _userManager.FindByIdAsync(changePasswordViewModel.Id.ToString());
    if (user == null)
        throw new KeyNotFoundException("User not found.");

    var result = await _userManager.ChangePasswordAsync(user, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);
    
    if (!result.Succeeded)
    {
        var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Unknown error";
        throw new InvalidOperationException($"Failed to change password: {errorMessage}");
    }

    return true;
}


public async Task<bool> CreateNewUser(UserCreateViewModel userCreateViewModel)
{
    var user = new User
    {
        Id = Guid.NewGuid(),
        UserName = userCreateViewModel.UserName ?? userCreateViewModel.Email,
        Email = userCreateViewModel.Email,
        FirstName = userCreateViewModel.FirstName,
        LastName = userCreateViewModel.LastName,
        PhoneNumber = userCreateViewModel.PhoneNumber,
        DateOfBirth = userCreateViewModel.DateOfBirth,
        IsActive = userCreateViewModel.IsActive
    };

    var result = await _userManager.CreateAsync(user, userCreateViewModel.Password);

    if (!result.Succeeded)
    {
        var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"Failed to create user: {errorMessages}");
    }

    return true;
}



    public Task<bool> DeleteUserById(Guid id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            var result = _context.SaveChanges();
            return Task.FromResult(result > 0);
        }
        throw new KeyNotFoundException("User not found.");
    }

    public async Task<List<UserViewModel>> GetAllUsers()
    {
        return await _context.Users
            .Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                DisplayName = user.DisplayName ?? string.Empty,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            })
            .ToListAsync();
    }

    public async Task<UserViewModel> GetUserIdBy(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            return new UserViewModel
            {
                Id = Guid.NewGuid(),
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                DisplayName = user.DisplayName ?? string.Empty,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };
        }
        throw new KeyNotFoundException("User not found.");
    }

    public Task<bool> UpdateUserById(Guid id, UserEditViewModel userEditViewModel)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.FirstName = userEditViewModel.FirstName ?? string.Empty;
            user.LastName = userEditViewModel.LastName ?? string.Empty;
            user.PhoneNumber = userEditViewModel.PhoneNumber;
            user.DateOfBirth = userEditViewModel.DateOfBirth ?? DateTime.Now;
            _context.Users.Update(user);
            var result = _context.SaveChanges();
            return Task.FromResult(result > 0);
        }
        throw new KeyNotFoundException("User not found!" + id.ToString());
    }
}

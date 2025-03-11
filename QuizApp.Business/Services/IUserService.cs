using Microsoft.AspNetCore.Identity;
using QuizApp.WebAPI.Models;

namespace QuizApp.Business.Services;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(User user, string password);

    Task<SignInResult> LoginAsync(string email, string password);
    
    Task<User?> GetUserByEmailAsync(string email);
}
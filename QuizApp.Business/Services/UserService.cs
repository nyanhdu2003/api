using Microsoft.AspNetCore.Identity;
using QuizApp.Data.Repositories;
using QuizApp.WebAPI.Models;

namespace QuizApp.Business.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;

    private readonly IUserRepository _userRepository;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<SignInResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return SignInResult.Failed;

        return await _signInManager.PasswordSignInAsync(user, password, false, false);
    }

    public async Task<IdentityResult> RegisterUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
}

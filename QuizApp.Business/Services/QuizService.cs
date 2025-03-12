using QuizApp.Business.ViewModels;
using QuizApp.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Business.Services;

public class QuizService
{
    private readonly QuizAppDbContext _context;

    public QuizService(QuizAppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy thông tin bài quiz cho user
    /// </summary>
    public async Task<QuizPrepareInfoViewModel?> PrepareQuizForUserAsync(PrepareQuizViewModel prepareQuizViewModel)
    {
        var quiz = await _context.Quizzes
            .Where(q => q.Id == prepareQuizViewModel.QuizId)
            .Select(q => new QuizPrepareInfoViewModel
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                Duration = q.Duration,
                ThumbnailUrl = q.ThumbnailUrl,
                QuizCode = prepareQuizViewModel.QuizCode,
                User = _context.Users
                    .Where(u => u.Id == prepareQuizViewModel.UserId)
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        DisplayName = u.DisplayName,
                        Email = u.Email,
                        UserName = u.UserName,
                        PhoneNumber = u.PhoneNumber,
                        DateOfBirth = u.DateOfBirth,
                        Avatar = u.Avatar,
                        IsActive = u.IsActive,
                        Roles = _context.Roles
                            .Where(r => r.UserRoles != null && r.UserRoles.Any(ur => ur.UserId == u.Id))
                            .Select(r => r.Name)
                            .ToList()
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        return quiz;
    }
}

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

    /// <summary>
    /// Get quiz information for user to take the quiz
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<QuizForTestViewModel> TakeQuizAsync(TakeQuizViewModel model)
    {
        var quiz = await _context.Quizzes
            .Where(q => q.Id == model.QuizId)
            .Select(q => new QuizForTestViewModel
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                QuizCode = _context.UserQuizzes
                    .Where(uq => uq.QuizId == model.QuizId && uq.UserId == model.UserId)
                    .Select(uq => uq.QuizCode)
                    .FirstOrDefault(),
                StartTime = DateTime.UtcNow,
                Duration = q.Duration,
                Questions = q.QuizQuestions
                    .Select(qq => new QuestionForTestViewModel
                    {
                        Id = qq.Question.Id,
                        Content = qq.Question.Content,
                        QuestionType = qq.Question.QuestionType,
                        Answers = qq.Question.Answers
                            .Select(a => new AnswerForTestViewModel
                            {
                                Id = a.Id,
                                Content = a.Content
                            }).ToList()
                    }).ToList()
            })
            .FirstOrDefaultAsync();
        return quiz!;
    }
}

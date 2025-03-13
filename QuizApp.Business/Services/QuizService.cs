using QuizApp.Business.ViewModels;
using QuizApp.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using QuizApp.WebAPI.Models;
using Microsoft.Extensions.Logging;
using static QuizApp.Business.Exceptions.QuizExceptions;

namespace QuizApp.Business.Services;

public class QuizService : IQuizService
{
    private readonly QuizAppDbContext _context;

    private readonly ILogger<QuizService> _logger;

    public QuizService(QuizAppDbContext context, ILogger<QuizService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy thông tin bài quiz cho user
    /// </summary>
    public async Task<QuizPrepareInfoViewModel?> PrepareQuizForUserAsync(PrepareQuizViewModel prepareQuizViewModel)
    {
        _logger.LogInformation("Preparing quiz for user {UserId}", prepareQuizViewModel.UserId);

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

        // Check if quiz is not found
        if (quiz == null)
        {
            _logger.LogError("Quiz not found for ID: {QuizId}", prepareQuizViewModel.QuizId);
            throw new QuizNotFoundException("Quiz not found.");
        }

        return quiz;
    }

    /// <summary>
    /// Get quiz information for user to take the quiz
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<QuizForTestViewModel> TakeQuizAsync(TakeQuizViewModel model)
    {
        _logger.LogInformation("Taking quiz {QuizId} for user {UserId}", model.QuizId, model.UserId);

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

        // Check if quiz is not found
        if (quiz == null)
        {
            _logger.LogError("Quiz not found for ID: {QuizId}", model.QuizId);
            throw new QuizNotFoundException("Quiz not found.");
        }

        return quiz;
    }

    /// <summary>
    /// Submit quiz
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> SubmitQuizAsync(QuizSubmissionViewModel model)
    {
        _logger.LogInformation("Submitting quiz {QuizId} for user {UserId}", model.QuizId, model.UserId);

        var userQuiz = await _context.UserQuizzes
            .FirstOrDefaultAsync(uq => uq.QuizId == model.QuizId && uq.UserId == model.UserId);

        // Check if user quiz is not found
        if (userQuiz == null)
        {
            _logger.LogError("UserQuiz not found for QuizId {QuizId} and UserId {UserId}", model.QuizId, model.UserId);
            throw new QuizSubmissionException("User quiz not found.");
        }

        // Check if user quiz is already submitted
        if (userQuiz.FinishAt.HasValue)
        {
            _logger.LogError("UserQuiz already submitted for QuizId {QuizId} and UserId {UserId}", model.QuizId, model.UserId);
            throw new QuizSubmissionException("User quiz already submitted.");
        }

        // Check if no answers provided
        if (model.UserAnswers == null || !model.UserAnswers.Any())
        {
            _logger.LogError("No answers provided for quiz {QuizId}", model.QuizId);
            throw new InvalidAnswerException("No answers provided.");
        }

        foreach (var answer in model.UserAnswers)
        {
            var correctAnswer = await _context.Answers
                .Where(a => a.QuestionId == answer.QuestionId && a.IsCorrect)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            // Check if answer is invalid
            if (correctAnswer == Guid.Empty)
            {
                _logger.LogError("Invalid answer for QuestionId {QuestionId}", answer.QuestionId);
                throw new InvalidAnswerException("Invalid answer for question.");
            }

            var isCorrect = correctAnswer == answer.AnswerId;

            var userAnswer = new UserAnswer
            {
                UserId = model.UserId,
                QuizId = model.QuizId,
                UserQuizId = userQuiz.Id,
                QuestionId = answer.QuestionId,
                AnswerId = answer.AnswerId,
                IsCorrect = isCorrect
            };

            _context.UserAnswers.Add(userAnswer);
        }

        userQuiz.FinishAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Get quiz result
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<QuizResultViewModel> GetQuizResultAsync(GetQuizResultViewModel model)
    {   
        _logger.LogInformation("Getting quiz result for quiz {QuizId} and user {UserId}", model.QuizId, model.UserId);

        var totalQuestions = await _context.QuizQuestions
            .CountAsync(qq => qq.QuizId == model.QuizId);

        var correctAnswers = await _context.UserAnswers
            .Where(ua => ua.UserId == model.UserId && ua.QuizId == model.QuizId && ua.IsCorrect)
            .CountAsync();

        // Check if no questions found
        if (totalQuestions == 0)
        {
            _logger.LogError("No questions found for quiz {QuizId}", model.QuizId);
            throw new QuizNotFoundException("No questions found for the quiz.");
        }

        var score = (correctAnswers * 100) / totalQuestions;

        return new QuizResultViewModel
        {
            QuizId = model.QuizId,
            UserId = model.UserId,
            CorrectAnswers = correctAnswers,
            TotalQuestions = totalQuestions,
            Score = score
        };
    }
}

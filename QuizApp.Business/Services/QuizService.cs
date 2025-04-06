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

    public async Task<QuizViewModel> GetQuizByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching quiz details for ID: {QuizId}", id);

        var quiz = await _context.Quizzes
            .Where(q => q.Id == id)
            .Select(q => new QuizViewModel
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description ?? string.Empty,
                Duration = q.Duration,
                IsActive = q.IsActive
            })
            .FirstOrDefaultAsync();
        if (quiz == null)
        {
            _logger.LogError("Quiz not found for ID: {QuizId}", id);
            throw new QuizNotFoundException("Quiz not found.");
        }
        return quiz;
    }

    public async Task<List<QuizViewModel>> GetAllQuizzesAsync()
    {
        _logger.LogInformation("Fetching all quizzes");

        return await _context.Quizzes
            .Select(q => new QuizViewModel
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description ?? string.Empty,
                Duration = q.Duration,
                IsActive = q.IsActive
            })
            .ToListAsync();
    }

    public async Task<bool> CreateQuizWithQuestionsAsync(QuizCreateViewModel model)
    {
        var quiz = new Quiz
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            Duration = model.Duration,
            IsActive = model.IsActive,
        };

        await _context.Quizzes.AddAsync(quiz);

        foreach (var question in model.QuestionIdWithOrders)
        {
            var quizQuestions = new QuizQuestion
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                QuestionId = question.QuestionId,
                Order = question.Order
            };

            await _context.AddAsync(quizQuestions);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteQuizAsync(Guid id)
    {
        var quiz = await _context.Quizzes
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null)
        {
            return false;
        }

        _context.Quizzes.Remove(quiz);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateQuizWithQuestionsAsync(Guid id, QuizEditViewModel model)
    {
        // Tìm quiz cần cập nhật
        var quiz = await _context.Quizzes
            .Include(q => q.QuizQuestions)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null)
        {
            _logger.LogError("Quiz not found with ID: {QuizId}", id);
            return false;
        }

        // Cập nhật thông tin quiz
        quiz.Title = model.Title;
        quiz.Description = model.Description;
        quiz.Duration = model.Duration;
        quiz.IsActive = model.IsActive;

        // Lấy danh sách QuizQuestions hiện có
        var existingQuizQuestions = quiz.QuizQuestions.ToList();

        // Nếu có danh sách câu hỏi mới
        if (model.QuestionIdWithOrders != null && model.QuestionIdWithOrders.Count > 0)
        {
            // Xóa danh sách câu hỏi cũ
            _context.QuizQuestions.RemoveRange(existingQuizQuestions);

            var newQuizQuestions = new List<QuizQuestion>();

            foreach (var item in model.QuestionIdWithOrders)
            {
                // Lấy đối tượng Question từ database
                var question = await _context.Questions
                    .FirstOrDefaultAsync(q => q.Id == item.QuestionId);

                if (question == null)
                {
                    _logger.LogWarning("Question ID {QuestionId} does not exist", item.QuestionId);
                    continue; // Bỏ qua câu hỏi không tồn tại
                }

                newQuizQuestions.Add(new QuizQuestion
                {
                    QuizId = id,
                    Quiz = quiz, // Bổ sung Quiz object
                    QuestionId = item.QuestionId,
                    Question = question, // Bổ sung Question object
                    Order = item.Order
                });
            }

            // Thêm danh sách mới vào database
            await _context.QuizQuestions.AddRangeAsync(newQuizQuestions);
        }

        // Lưu thay đổi vào database
        _logger.LogInformation("Quiz {QuizId} updated successfully", id);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddQuestionToQuiz(QuizQuestionCreateViewModel model)
    {
        var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == model.QuizId);

        if (quiz == null)
        {
            System.Console.WriteLine("No quiz found");
            return false;
        }

        var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == model.QuestionId);

        if (question == null)
        {
            System.Console.WriteLine("No question found");
            return false;
        }

        var newQuestion = new QuizQuestion
        {
            Id = Guid.NewGuid(),
            QuizId = quiz.Id,
            QuestionId = question.Id,
            Quiz = quiz,
            Question = question,
            Order = model.Order
        };

        await _context.QuizQuestions.AddAsync(newQuestion);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteQuestionFromQuiz(Guid id, Guid questionId)
    {
        // Tìm xem câu hỏi có tồn tại hay không
        var quiz = await _context.Quizzes
            .Include(q => q.QuizQuestions)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null)
        {
            System.Console.WriteLine("No quiz found");
            return false;
        }

        // Tìm câu hỏi cần xóa khỏi quiz
        var quizQuestion = await _context.QuizQuestions
            .FirstOrDefaultAsync(qq => qq.QuizId == id && qq.QuestionId == questionId);

        if (quizQuestion == null)
        {
            System.Console.WriteLine("No questions found in this quiz");
            return false;
        }

        // Xóa quan hệ giữa quiz và question
        _context.QuizQuestions.Remove(quizQuestion);

        // Lưu thay đổi vào database 
        return await _context.SaveChangesAsync() > 0;
    }

   public async Task<QuizPrepareInfoViewModel?> PrepareQuizForUser(PrepareQuizViewModel prepareQuizViewModel)
{
    _logger.LogInformation("Preparing quiz for user: {UserId}, Quiz: {QuizId}", prepareQuizViewModel.UserId, prepareQuizViewModel.QuizId);

    // Kiểm tra đầu vào
    if (prepareQuizViewModel.QuizId == Guid.Empty ||
        prepareQuizViewModel.UserId == Guid.Empty ||
        string.IsNullOrWhiteSpace(prepareQuizViewModel.QuizCode))
    {
        _logger.LogWarning("Invalid request data.");
        return null;
    }

    // Tìm quiz trong database
    var quiz = await _context.Quizzes.FindAsync(prepareQuizViewModel.QuizId);
    if (quiz == null)
    {
        _logger.LogWarning("Quiz {QuizId} not found.", prepareQuizViewModel.QuizId);
        return null;
    }

    // Tìm user trong database
    var user = await _context.Users.FindAsync(prepareQuizViewModel.UserId);
    if (user == null)
    {
        _logger.LogWarning("User {UserId} not found.", prepareQuizViewModel.UserId);
        return null;
    }

    // Kiểm tra xem UserQuiz đã tồn tại chưa
    var existingEntry = await _context.UserQuizzes
        .FirstOrDefaultAsync(uq => uq.UserId == user.Id && uq.QuizId == quiz.Id);

    if (existingEntry == null) // Chỉ thêm nếu chưa có
    {
        var userQuiz = new UserQuizz
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            QuizId = quiz.Id,
            QuizCode = prepareQuizViewModel.QuizCode,
            StartAt = DateTime.UtcNow
        };

        _context.UserQuizzes.Add(userQuiz);
        await _context.SaveChangesAsync();
    }

    // Chuẩn bị dữ liệu phản hồi
    var response = new QuizPrepareInfoViewModel
    {
        Id = quiz.Id,
        Title = quiz.Title,
        Description = quiz.Description ?? string.Empty,
        Duration = quiz.Duration,
        ThumbnailUrl = quiz.ThumbnailUrl,
        QuizCode = prepareQuizViewModel.QuizCode,
        User = new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DisplayName = user.DisplayName ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            Avatar = user.Avatar,
            IsActive = user.IsActive,
            Roles = await _context.Roles
                .Where(r => r.UserRoles != null && r.UserRoles.Any(ur => ur.UserId == user.Id))
                .Select(r => r.Name)
                .ToListAsync()
        }
    };

    _logger.LogInformation("Quiz {QuizId} prepared successfully for user {UserId}.", prepareQuizViewModel.QuizId, prepareQuizViewModel.UserId);
    return response;
}

}

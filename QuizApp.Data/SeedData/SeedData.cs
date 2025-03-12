using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.SeedData
{
    public static class SeedData
    {
        public const string testconstant = "This is my answer";

        public const string multipleChoice = "Multiple Choices";


        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new QuizAppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<QuizAppDbContext>>()
            ))
            {
                // Đảm bảo database đã được tạo và cập nhật
                await context.Database.MigrateAsync();

                        // Kiểm tra nếu dữ liệu đã tồn tại thì không cần seed
                        if (await context.Quizzes.AnyAsync() || await context.Questions.AnyAsync() || await context.Answers.AnyAsync())
                        {
                            Console.WriteLine("⚠️ Dữ liệu đã tồn tại, không cần seed.");
                            return;
                        }

                        if (await context.Users.AnyAsync()) return;

                        // Tạo dữ liệu Users
                        var users = new[]
                        {
                    new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", DisplayName = "John Doe", Email = "john@example.com", UserName = "john.doe", DateOfBirth = new DateTime(2000, 1, 1), IsActive = true },
                    new User { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", DisplayName = "Jane Smith", Email = "jane@example.com", UserName = "jane.smith", DateOfBirth = new DateTime(1998, 8, 15), IsActive = true }
                };
                        await context.Users.AddRangeAsync(users);
                        await context.SaveChangesAsync();

                        // Tạo dữ liệu Quizzes
                        var quizzes = new[]
                        {
                    new Quiz { Id = Guid.NewGuid(), Title = "Math Quiz", Description = "Basic math questions", Duration = 600, IsActive = true },
                    new Quiz { Id = Guid.NewGuid(), Title = "Science Quiz", Description = "Basic science facts", Duration = 900, IsActive = true },
                    new Quiz { Id = Guid.NewGuid(), Title = "History Quiz", Description = "World history questions", Duration = 1200, IsActive = true },
                    new Quiz { Id = Guid.NewGuid(), Title = "Programming Quiz", Description = "Basic coding concepts", Duration = 1800, IsActive = true },
                    new Quiz { Id = Guid.NewGuid(), Title = "Geography Quiz", Description = "Geography trivia", Duration = 1500, IsActive = true }
                };
                        await context.Quizzes.AddRangeAsync(quizzes);
                        await context.SaveChangesAsync();

                        // Tạo dữ liệu Questions
                        var questions = new[]
                        {
                    new Question { Id = Guid.NewGuid(), Content = "What is 2 + 2?", QuestionType = "SingleChoice", IsActive = true },
                    new Question { Id = Guid.NewGuid(), Content = "What is the capital of France?", QuestionType = "SingleChoice", IsActive = true },
                    new Question { Id = Guid.NewGuid(), Content = "Who developed the theory of relativity?", QuestionType = "SingleChoice", IsActive = true },
                    new Question { Id = Guid.NewGuid(), Content = "What is the largest planet in our solar system?", QuestionType = "SingleChoice", IsActive = true },
                    new Question { Id = Guid.NewGuid(), Content = "Which year did World War II end?", QuestionType = "SingleChoice", IsActive = true }
                };
                        await context.Questions.AddRangeAsync(questions);
                        await context.SaveChangesAsync(); // Lưu vào DB trước khi lấy ID

                        // Tạo dữ liệu Answers
                        var answers = new List<Answer>
                {
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[0].Id, Content = "4", IsCorrect = true, IsActive = true },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[0].Id, Content = "5", IsCorrect = false, IsActive = true },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[1].Id, Content = "Paris", IsCorrect = true, IsActive = true },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[1].Id, Content = "London", IsCorrect = false, IsActive = true },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[2].Id, Content = "Albert Einstein", IsCorrect = true, IsActive = true },
                };
                        await context.Answers.AddRangeAsync(answers);
                        await context.SaveChangesAsync();

                // Seed dữ liệu từ file SQL
                try
                {
                    var sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations", "SeedUserQuizData.sql");

                    if (File.Exists(sqlFilePath))
                    {
                        var sql = await File.ReadAllTextAsync(sqlFilePath);
                        await context.Database.ExecuteSqlRawAsync(sql);
                    }
                    else
                    {
                        Console.WriteLine($"Không tìm thấy file: {sqlFilePath}");
                    }

                    Console.WriteLine("Seed dữ liệu UserQuiz thành công!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi seed dữ liệu: {ex.Message}");
                }
            }
        }

    }
}


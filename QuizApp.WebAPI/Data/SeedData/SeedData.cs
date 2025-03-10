using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
                // Kiểm tra xem có dữ liệu chưa, nếu có thì không thêm nữa
                if (await context.Quizzes.AnyAsync() || await context.Questions.AnyAsync() || await context.Answers.AnyAsync())
                {
                    return;
                }

                // Tạo dữ liệu cho Quiz
                var quizzes = new[]
                {
                    new Quiz { Id = Guid.NewGuid(), Description = "These are Math quizzes", Title = "Math Quiz", Duration = 60, IsActive = true },
                    new Quiz { Id = Guid.NewGuid(), Description = "These are History quizzes", Title = "History Quiz", Duration = 60, IsActive = false },
                    new Quiz { Id = Guid.NewGuid(), Description = "These are English quizzes", Title = "English Quiz", Duration = 60, IsActive = true },
                    new Quiz { Id = Guid.NewGuid(), Description = "These are Science quizzes", Title = "Science Quiz", Duration = 60, IsActive = true },
                    new Quiz { Id = Guid.NewGuid(), Description = "These are Literature quizzes", Title = "Literature Quiz",  Duration = 60, IsActive = true },
                };
                await context.Quizzes.AddRangeAsync(quizzes);
                await context.SaveChangesAsync();

                // Tạo dữ liệu cho Question
                var questions = new[]
                {
                    new Question { Id = Guid.NewGuid(), Content = "These are Math Questionzes?", QuestionType = multipleChoice, IsActive = true },
                    new Question { Id = Guid.NewGuid(), Content = "These are History Questionzes?", QuestionType = multipleChoice, IsActive = false },
                    new Question { Id = Guid.NewGuid(), Content = "These are English Questionzes?", QuestionType = multipleChoice,IsActive = true },
                    new Question { Id = Guid.NewGuid(), Content = "These are Science Questionzes?", QuestionType = multipleChoice, IsActive = true },
                    new Question { Id = Guid.NewGuid(), Content = "These are Literature quizzes?", QuestionType = multipleChoice, IsActive = true },
                };
                await context.Questions.AddRangeAsync(questions);
                await context.SaveChangesAsync();
                
                // Tạo dữ liệu cho Answer
                var answers = new[]
                {
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[0].Id, Content = testconstant, IsCorrect = true, IsActive = true },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[1].Id, Content = testconstant, IsCorrect = true, IsActive = false },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[2].Id, Content = testconstant, IsCorrect = true, IsActive = true },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[3].Id, Content = testconstant, IsCorrect = true, IsActive = true },
                    new Answer { Id = Guid.NewGuid(), QuestionId = questions[4].Id, Content = testconstant, IsCorrect = true,  IsActive = true },
                };
                await context.Answers.AddRangeAsync(answers);
                await context.SaveChangesAsync();
            }
        }
    }
}


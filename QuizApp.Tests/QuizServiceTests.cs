using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Tests
{
    [TestFixture]
    public class QuizServiceTests
    {
        private QuizAppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuizAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Tạo database mới mỗi lần chạy test
                .Options;

            _context = new QuizAppDbContext(options);
            _context.Database.EnsureCreated(); // Đảm bảo DB được tạo
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted(); // Xóa DB sau khi test
            _context.Dispose();
        }

        [Test]
        public async Task AddQuiz_Should_Add_Quiz_To_Database()
        {
            // Arrange
            var quiz = CreateSampleQuiz();

            // Act
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Assert
            var addedQuiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Title == "Sample Quiz");
            Assert.That(addedQuiz, Is.Not.Null);
            Assert.That(addedQuiz.Title, Is.EqualTo(quiz.Title));
        }

        [Test]
        public async Task GetQuizById_Should_Return_Correct_Quiz()
        {
            // Arrange
            var quiz = CreateSampleQuiz();
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            var foundQuiz = await _context.Quizzes.FindAsync(quiz.Id);

            // Assert
            Assert.That(foundQuiz, Is.Not.Null);
            Assert.That(foundQuiz.Id, Is.EqualTo(quiz.Id));
        }

        [Test]
        public async Task UpdateQuiz_Should_Modify_Quiz_Details()
        {
            // Arrange
            var quiz = CreateSampleQuiz();
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            quiz.Title = "Updated Quiz";
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();

            // Assert
            var updatedQuiz = await _context.Quizzes.FindAsync(quiz.Id);
            Assert.That(updatedQuiz, Is.Not.Null);
            Assert.That(updatedQuiz!.Title, Is.EqualTo("Updated Quiz"));
        }

        [Test]
        public async Task DeleteQuiz_Should_Remove_Quiz_From_Database()
        {
            // Arrange
            var quiz = CreateSampleQuiz();
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            // Assert
            var deletedQuiz = _context.Quizzes.FindAsync(quiz.Id);
            Assert.That(await deletedQuiz, Is.Null);
        }

        private Quiz CreateSampleQuiz()
        {
            return new Quiz
            {
                Id = Guid.NewGuid(),
                Title = "Sample Quiz",
                Description = "This is a test quiz",
                Duration = 60,
                IsActive = true
            };
        }
    }
}

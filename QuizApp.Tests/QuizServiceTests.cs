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

        // Test xóa User dẫn đến xóa UserQuiz (Cascade Delete)
        [Test]
        public async Task CascadeDelete_User_Should_Remove_UserQuiz()
        {
            var user = new User { Id = Guid.NewGuid(), UserName = "testuser", FirstName = "Test", LastName = "User", DisplayName = "Test User", DateOfBirth = DateTime.Now, IsActive = true };
            var quiz = CreateSampleQuiz();

            _context.Users.Add(user);
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            var userQuiz = new UserQuiz { UserId = user.Id, QuizId = quiz.Id, QuizCode = "123456" };
            _context.UserQuizzes.Add(userQuiz);
            await _context.SaveChangesAsync();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            var userQuizAfterDelete = await _context.UserQuizzes.FirstOrDefaultAsync(uq => uq.UserId == user.Id);
            Assert.That(userQuizAfterDelete, Is.Null);
        }

        // Test tạo quan hệ Many-to-Many giữa User và Quiz
        [Test]
        public async Task AddUserQuiz_Should_Create_ManyToMany_Relationship()
        {
            var user = new User { Id = Guid.NewGuid(), UserName = "testuser2", FirstName = "Test", LastName = "User", DisplayName = "Test User 2", DateOfBirth = DateTime.Now, IsActive = true };
            var quiz = CreateSampleQuiz();

            _context.Users.Add(user);
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            var userQuiz = new UserQuiz { UserId = user.Id, QuizId = quiz.Id, QuizCode = "132356" };
            _context.UserQuizzes.Add(userQuiz);
            await _context.SaveChangesAsync();

            var result = await _context.UserQuizzes.FirstOrDefaultAsync(uq => uq.UserId == user.Id && uq.QuizId == quiz.Id);
            Assert.That(result, Is.Not.Null);
        }
    }
}

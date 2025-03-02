using Microsoft.EntityFrameworkCore;
using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.Data;

public class QuizAppDbContext : DbContext
{   
    // DbContext used for managing database connections & retrieving, deleting, ... data.
    public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) : base(options) { }

    // DbContext represents for a table in database, used for CRUD.
    public DbSet<Quiz> Quizzes { get; set; }

    public DbSet<Answer> Answers { get; set; }

    public DbSet<Question> Questions { get; set; }
}

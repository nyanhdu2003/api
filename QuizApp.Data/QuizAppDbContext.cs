using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models.Models;
using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.Data;

public class QuizAppDbContext : IdentityDbContext<User, Role, Guid>
{
    // DbContext used for managing database connections & retrieving, deleting, ... data.
    public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) : base(options) { }

    // DbContext represents for a table in database, used for CRUD.
    public DbSet<Quiz> Quizzes { get; set; }

    public DbSet<Answer> Answers { get; set; }

    public DbSet<Question> Questions { get; set; }

    public override DbSet<Role> Roles { get; set; }

    public override DbSet<User> Users { get; set; }

    public DbSet<UserQuiz> UserQuizzes { get; set; }

    public DbSet<UserAnswer> UserAnswers { get; set; }

    public DbSet<QuizQuestion> QuizQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) // Override phương thức cấu hình database khi tạo model
    {
        base.OnModelCreating(builder); // Gọi phương thức gốc để đảm bảo các cấu hình cơ bản vẫn được áp dụng

        // Ngăn Identity đổi tên bảng mặc định
        builder.Entity<User>().ToTable("Users");
        builder.Entity<Quiz>().ToTable("Quizzes");
        builder.Entity<Question>().ToTable("Questions");
        builder.Entity<Answer>().ToTable("Answers");
        builder.Entity<UserQuiz>().ToTable("UserQuizz");

        builder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Cấu hình quan hệ Many-to-Many giữa User và Quiz thông qua UserQuiz
        builder.Entity<UserQuiz>()
            .HasKey(uc => new { uc.UserId, uc.QuizId }); // Định nghĩa Id là khóa chính của bảng UserQuiz

        builder.Entity<UserQuiz>()
            .HasOne(uc => uc.User) // Một UserQuiz có một User
            .WithMany(u => u.UserQuizzes) // Một User có thể có nhiều UserQuiz (nhiều lần làm bài)
            .HasForeignKey(uc => uc.UserId) // UserQuiz có khóa ngoại UserId tham chiếu đến User
            .OnDelete(DeleteBehavior.Cascade); // Nếu User bị xóa thì tất cả UserQuiz liên quan cũng bị xóa

        builder.Entity<UserQuiz>()
            .HasOne(uc => uc.Quiz) // Một UserQuiz có một Quiz
            .WithMany(q => q.UserQuizzes) // Một Quiz có thể có nhiều UserQuiz (nhiều người làm bài)
            .HasForeignKey(uc => uc.QuizId) // UserQuiz có khóa ngoại QuizId tham chiếu đến Quiz
            .OnDelete(DeleteBehavior.Cascade); // Nếu Quiz bị xóa thì tất cả UserQuiz liên quan cũng bị xóa

        builder.Entity<UserAnswer>()
            .HasKey(x => new { x.UserQuizId, x.AnswerId, x.QuestionId });

        // Cấu hình quan hệ One-to-Many giữa UserQuiz và UserAnswer
        builder.Entity<UserAnswer>()
            .HasOne(ua => ua.UserQuiz) // Một UserAnswer thuộc về một UserQuiz
            .WithMany(uq => uq.UserAnswers) // Một UserQuiz có nhiều UserAnswer
            .HasForeignKey(ua => new { ua.UserId, ua.QuizId }) // UserAnswer có khóa ngoại UserQuizId tham chiếu đến UserQuiz
            .OnDelete(DeleteBehavior.NoAction); // Nếu UserQuiz bị xóa, tất cả câu trả lời cũng bị xóa

        // Cấu hình quan hệ giữa UserAnswer và Question
        builder.Entity<UserAnswer>()
            .HasOne(ua => ua.Question) // Một UserAnswer thuộc về một Question
            .WithMany() // Một Question có thể có nhiều UserAnswer
            .HasForeignKey(ua => ua.QuestionId) // UserAnswer có khóa ngoại QuestionId tham chiếu đến Question
            .OnDelete(DeleteBehavior.Restrict); // Nếu Question bị xóa, UserAnswer không bị xóa

        // Cấu hình quan hệ giữa UserAnswer và Answer
        builder.Entity<UserAnswer>()
            .HasOne(ua => ua.Answer) // Một UserAnswer thuộc về một Answer
            .WithMany() // Một Answer có thể xuất hiện trong nhiều UserAnswer
            .HasForeignKey(ua => ua.AnswerId) // UserAnswer có khóa ngoại AnswerId tham chiếu đến Answer
            .OnDelete(DeleteBehavior.Restrict); // Nếu Answer bị xóa, UserAnswer không bị xóa

        // Relationship Quiz-Question: N:N
        builder.Entity<QuizQuestion>()
            .HasKey(x => new { x.QuizId, x.QuestionId });

        // Relationship Quiz - QuizQuestion: 1:N
        builder.Entity<QuizQuestion>()
            .HasOne(qq => qq.Quiz)
            .WithMany(q => q.QuizQuestions)
            .HasForeignKey(qq => qq.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship Question - QuizQuestion: 1:N
        builder.Entity<QuizQuestion>()
            .HasOne(qq => qq.Question)
            .WithMany(q => q.QuizQuestions)
            .HasForeignKey(qq => qq.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Primary key for UserRoles
        builder.Entity<UserRoles>()
            .HasKey(x => new { x.UserId, x.RoleId });
        
        // Relationship Role - UserRoles: 1:N
        builder.Entity<UserRoles>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Relationship User - UserRoles: 1:N
        builder.Entity<UserRoles>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

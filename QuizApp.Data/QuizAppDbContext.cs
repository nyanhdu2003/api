using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models.Models;
using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.Data
{
    public class QuizAppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) : base(options) { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserQuizz> UserQuizzes { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Đổi tên bảng cho Identity
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<Quiz>().ToTable("Quizzes");
            builder.Entity<Question>().ToTable("Questions");
            builder.Entity<Answer>().ToTable("Answers");
            builder.Entity<UserQuizz>().ToTable("UserQuizzes");

            // ✅ Cấu hình Answer - Question (1:N)
            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Cấu hình Many-to-Many giữa User và Quiz thông qua UserQuizz
            builder.Entity<UserQuizz>()
                .HasKey(uc => new { uc.UserId, uc.QuizId });

            builder.Entity<UserQuizz>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserQuizzes)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Ngăn vòng lặp xóa

            builder.Entity<UserQuizz>()
                .HasOne(uc => uc.Quiz)
                .WithMany(q => q.UserQuizzes)
                .HasForeignKey(uc => uc.QuizId)
                .OnDelete(DeleteBehavior.Restrict); // Ngăn vòng lặp xóa

            // ✅ Cấu hình UserAnswer
            builder.Entity<UserAnswer>()
                .HasKey(ua => new { ua.UserId, ua.QuizId, ua.AnswerId, ua.QuestionId }); // Đặt khóa chính đúng

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.UserQuizz)
                .WithMany(uq => uq.UserAnswers)
                .HasForeignKey(ua => new { ua.UserId, ua.QuizId }) // Khóa ngoại đúng
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.Question)
                .WithMany()
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.Answer)
                .WithMany()
                .HasForeignKey(ua => ua.AnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Cấu hình Many-to-Many giữa Quiz và Question thông qua QuizQuestion
            builder.Entity<QuizQuestion>()
                .HasKey(x => new { x.QuizId, x.QuestionId });

            builder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Question)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Cấu hình UserRoles (Many-to-Many giữa User và Role)
            builder.Entity<UserRoles>()
                .HasBaseType<IdentityUserRole<Guid>>(); // Kế thừa đúng từ IdentityUserRole<Guid>

            builder.Entity<UserRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserRoles>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

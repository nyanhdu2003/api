using QuizApp.Models.Models;
using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;

namespace QuizApp.Data.Repositories;

public interface IUnitOfWork : IDisposable
{

    #region Repositories
    QuizAppDbContext Context { get; }

    IGenericRepository<Quiz> QuizRepository { get; }

    IGenericRepository<Question> QuestionRepository { get; }

    IGenericRepository<UserQuiz> UserQuizRepository { get; }

    IGenericRepository<QuizQuestion> QuizQuestionRepository { get; }

    IGenericRepository<UserAnswer> UserAnswerRepository { get; }

    IGenericRepository<User> UserRepository { get; }

    IGenericRepository<Role> RoleRepository { get; }

    IGenericRepository<Answer> AnswerRepository { get; }

    IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class, IBaseEntity;

    #endregion

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}

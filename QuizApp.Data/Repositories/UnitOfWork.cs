using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;
using Microsoft.EntityFrameworkCore.Storage;
using QuizApp.Models.Models;

namespace QuizApp.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{   
    private readonly QuizAppDbContext _context;
    private IDbContextTransaction? _transaction;

    // Dictionary để lưu trữ các repository
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(QuizAppDbContext context)
    {   
        _context = context;

        // Khởi tạo các repository cụ thể
        QuizRepository = new GenericRepository<Quiz>(_context);
        QuestionRepository = new GenericRepository<Question>(_context);
        UserQuizRepository = new GenericRepository<UserQuizz>(_context);
        QuizQuestionRepository = new GenericRepository<QuizQuestion>(_context);
        UserAnswerRepository = new GenericRepository<UserAnswer>(_context);
        UserRepository = new GenericRepository<User>(_context);
        RoleRepository = new GenericRepository<Role>(_context);
        AnswerRepository = new GenericRepository<Answer>(_context);
    }

    public QuizAppDbContext Context => _context;

    // Implement các repository
    public IGenericRepository<Quiz> QuizRepository { get; }
    public IGenericRepository<Question> QuestionRepository { get; }
    public IGenericRepository<UserQuizz> UserQuizRepository { get; }
    public IGenericRepository<QuizQuestion> QuizQuestionRepository { get; }
    public IGenericRepository<UserAnswer> UserAnswerRepository { get; }
    public IGenericRepository<User> UserRepository { get; }
    public IGenericRepository<Role> RoleRepository { get; }
    public IGenericRepository<Answer> AnswerRepository { get; }

    // Phương thức tạo repository động
    public IGenericRepository<T> GenericRepository<T>() where T : class, IBaseEntity
    {
        var type = typeof(T);
        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new GenericRepository<T>(_context);
            _repositories[type] = repositoryInstance;
        }
        return (IGenericRepository<T>)_repositories[type];
    }

    // Bắt đầu transaction
    public async Task BeginTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
    }

    // Commit transaction
    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // Rollback transaction
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // Lưu thay đổi vào database
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    // Giải phóng tài nguyên
    private bool _disposed = false;
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
                _transaction?.Dispose();
            }
            _disposed = true;
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~UnitOfWork()
    {
        Dispose(false);
    }
}

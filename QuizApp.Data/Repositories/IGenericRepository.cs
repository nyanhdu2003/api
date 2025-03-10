using System.Linq.Expressions;

namespace QuizApp.Data.Repositories;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetAll();

    Task<IEnumerable<T>> GetAllAsync();

    T? GetById(Guid Id);

    Task<T?> GetByIdAsync(Guid Id);

    void Add(T entity);

    void Update(T entity);

    void Delete(Guid Id);

    void Delete(T entity);

    IQueryable<T> GetQuery();

    IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);

    IQueryable<T> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "");
}

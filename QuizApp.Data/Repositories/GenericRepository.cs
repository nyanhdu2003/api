using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models.Models;
using QuizApp.WebAPI.Data;

namespace QuizApp.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
{
    private readonly QuizAppDbContext _context;

    private readonly DbSet<T> _dbSet;

    public GenericRepository(QuizAppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(Guid Id)
    {
        var entity = _dbSet.FirstOrDefault(d => d.Id == Id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }

    public void Delete(T entity)
    {
        if (entity != null)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }

    public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return orderBy != null ? orderBy(query) : query;
    }

    public IEnumerable<T> GetAll()
    {
        return [.. _dbSet];
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public T? GetById(Guid Id)
    {
        return _dbSet.FirstOrDefault(e => e.Id == Id);
    }

    public async Task<T?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == Id);
    }

    public IQueryable<T> GetQuery()
    {
        return _dbSet.AsQueryable();
    }

    public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }
}

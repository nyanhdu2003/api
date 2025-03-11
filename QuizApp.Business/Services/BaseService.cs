using System.Linq.Expressions;
using QuizApp.Data.Repositories;
using QuizApp.Models.Models;

namespace QuizApp.Business.Services;

public class BaseService<T> : IBaseService<T> where T : class, IBaseEntity
{
    private readonly IGenericRepository<T> _genericRepository;

    private readonly IUnitOfWork _unitOfWork;

    public BaseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GenericRepository<T>();
    }

    public async Task<int> AddAsync(T entity)
    {
        _genericRepository.Add(entity);
        return await _unitOfWork.SaveChangesAsync();
    }

    public bool Delete(Guid id)
    {
        var entity = _genericRepository.GetById(id);
        if (entity != null)
        {
            _genericRepository.Delete(entity);
        }

        return _unitOfWork.SaveChanges() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _genericRepository.GetByIdAsync(id);
        if (entity != null)
        {
            _genericRepository.Delete(entity);
        }

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        _genericRepository.Delete(entity);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
    }

    public Task<PaginatedResult<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", int pageIndex = 1, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAsync(Guid id)
    {
        return _genericRepository.GetByIdAsync(id);
    }

    public Task<bool> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}

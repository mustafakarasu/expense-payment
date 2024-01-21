using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstractions
{
    public interface IRepositoryBase<T>
    {
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Queryable();
    }
}

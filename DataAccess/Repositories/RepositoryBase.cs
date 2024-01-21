using System.Linq.Expressions;
using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ExpenseContext Context;

        public RepositoryBase(ExpenseContext context)
        {
            Context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().AnyAsync(predicate);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).ToListAsync();
        }

        public IQueryable<T> Queryable()
        {
            return Context.Set<T>();
        }
    }
}

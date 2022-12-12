using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetById(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public Task AddRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public Task DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}

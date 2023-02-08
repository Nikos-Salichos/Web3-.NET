using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MsqlDbContext _dbContext;
        // protected readonly PostgreSqlDbContext _dbContext;
        private readonly IDistributedCache _distributedCache;

        public GenericRepository(MsqlDbContext dbContext, IDistributedCache distributedCache)
        {
            _dbContext = dbContext;
            _distributedCache = distributedCache;
        }

        public async Task<T> GetById(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(int pageSize = 1, int pageNumber = 1)
        {
            return await _dbContext.Set<T>()
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public Task Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public async Task<bool> Delete(string id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task DeleteRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }
    }
}

using System.Linq.Expressions;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(long id);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetAll(int pageSize = 3, int pageNumber = 1);

        Task<T> Add(T entity);
        Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);

        Task Update(T entity);
        Task UpdateRange(IEnumerable<T> entities);

        Task<bool> Delete(string id);
        Task DeleteRange(IEnumerable<T> entities);
    }
}

using System.Linq.Expressions;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(string id);
        Task<T> Find(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);

        Task Update(T entity);
        Task UpdateRange(IEnumerable<T> entities);

        Task Delete(T entity);
        Task DeleteRange(IEnumerable<T> entities);
    }
}

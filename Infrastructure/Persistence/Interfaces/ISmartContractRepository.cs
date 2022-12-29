using Domain.Models;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Interfaces
{
    public interface ISmartContractRepository : IGenericRepository<SmartContract>
    {
        Task<IEnumerable<SmartContract>> GetSmartContracts();
        Task<SmartContract> GetSmartContractAsync(long id);
        Task<IEnumerable<SmartContract>> FindSmartContractAsync(Expression<Func<SmartContract, bool>> predicate);
    }
}

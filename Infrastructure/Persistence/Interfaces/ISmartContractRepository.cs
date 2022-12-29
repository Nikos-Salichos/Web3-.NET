using Domain.Models;

namespace Infrastructure.Persistence.Interfaces
{
    public interface ISmartContractRepository : IGenericRepository<SmartContract>
    {
        Task<IEnumerable<SmartContract>> GetSmartContracts();

        Task<SmartContract> GetSmartContractAsync(long id);
    }
}

using Domain.Models;

namespace Infrastructure.Persistence.Interfaces
{
    public interface ISmartContractRepository : IGenericRepository<SmartContract>
    {
        IEnumerable<SmartContract> GetSmartContracts();
    }
}

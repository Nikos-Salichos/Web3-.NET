using Domain.Models;

namespace Application.Interfaces
{
    public interface ISmartContractService
    {
        Task<IEnumerable<SmartContract>> GetSmartContracts();
    }
}

using Domain.Models;

namespace Application.Interfaces
{
    public interface ISmartContractService
    {
        IEnumerable<SmartContract> GetSmartContracts();
    }
}

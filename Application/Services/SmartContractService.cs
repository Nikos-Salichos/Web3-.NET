using Application.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;

namespace Application.Services
{
    public class SmartContractService : ISmartContractService
    {
        private readonly ISmartContractRepository _smartContractRepository;

        public SmartContractService(ISmartContractRepository smartContractRepository)
        {
            _smartContractRepository = smartContractRepository;
        }

        public Task<IEnumerable<SmartContract>> GetSmartContracts()
        {
            return _smartContractRepository.GetSmartContracts();
        }
    }
}

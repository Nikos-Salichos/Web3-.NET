using Domain.Models;
using Infrastructure.Context;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class SmartContractRepository : GenericRepository<SmartContract>, ISmartContractRepository
    {
        public SmartContractRepository(RepositoryContext smartContractDbContext) : base(smartContractDbContext) { }

        public async Task<IEnumerable<SmartContract>> GetSmartContracts()
        {
            var allSmartContracts = await GetAll();
            return allSmartContracts;
        }
    }
}

using Domain.Models;
using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class SmartContractRepository : GenericRepository<SmartContract>, ISmartContractRepository
    {
        public SmartContractRepository(MsqlDbContext smartContractDbContext) : base(smartContractDbContext) { }

        public async Task<IEnumerable<SmartContract>> GetSmartContracts()
        {
            var allSmartContracts = await GetAll();
            return allSmartContracts;
        }

        public async Task<SmartContract> GetSmartContractAsync(long id)
        {
            var smartContract = await GetById(id);
            return smartContract;
        }

        public async Task<IEnumerable<SmartContract>> FindSmartContractAsync(Expression<Func<SmartContract, bool>> predicate)
        {
            var smartContract = await Find(predicate);
            return smartContract;
        }
    }
}

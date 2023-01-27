using Domain.Models;
using Infrastructure.Persistence.Cache;
using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class SmartContractRepository : GenericRepository<SmartContract>, ISmartContractRepository
    {
        private readonly IDistributedCache _distributedCache;

        public SmartContractRepository(MsqlDbContext smartContractDbContext, IDistributedCache distributedCache)
            : base(smartContractDbContext, distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<SmartContract>> GetSmartContracts()
        {
            var allSmartContracts = await GetAll();
            return allSmartContracts;
        }

        public async Task<SmartContract> GetSmartContractAsync(long id)
        {
            var cacheSmartContract = await _distributedCache.GetRecordAsync<SmartContract>(id.ToString());
            if (cacheSmartContract != null)
            {
                return cacheSmartContract;
            }
            var smartContract = await GetById(id);
            await _distributedCache.SetRecordAsync(id.ToString(), smartContract);
            return smartContract;
        }

        public async Task<IEnumerable<SmartContract>> FindSmartContractAsync(Expression<Func<SmartContract, bool>> predicate)
        {
            var smartContract = await Find(predicate);
            return smartContract;
        }
    }
}

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
        private const string redisAllSmartContractsKey = "allsmartcontracts";
        private readonly IDistributedCache _distributedCache;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public SmartContractRepository(MsSqlDbContext smartContractDbContext,
                                       IDistributedCache distributedCache,
                                       IUnitOfWorkRepository unitOfWorkRepository)
                                       : base(smartContractDbContext, distributedCache)
        {
            _distributedCache = distributedCache;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<IEnumerable<SmartContract>> GetSmartContracts(int pageSize, int pageNumber)
        {
            var cacheSmartContracts = await _distributedCache.GetRecordAsync<IEnumerable<SmartContract>>("allsmartcontracts");
            if (cacheSmartContracts == null)
            {
                cacheSmartContracts = await GetAll();
                await _distributedCache.SetRecordAsync(redisAllSmartContractsKey, cacheSmartContracts);
            }
            return cacheSmartContracts.Skip((pageNumber - 1) * pageSize).Take(pageSize);
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

        public async Task<SmartContract> AddSmartContractAsync(SmartContract smartContract)
        {
            await _unitOfWorkRepository.SmartContractRepository.Add(smartContract);
            await _unitOfWorkRepository.SaveChangesAsync();

            var allSmartContracts = await GetAll();
            await _distributedCache.SetRecordAsync(redisAllSmartContractsKey, allSmartContracts);

            return smartContract;
        }
    }
}

using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private MsSqlDbContext _msSqlContext;
        private IDistributedCache _distributedCache;
        private ISmartContractRepository _smartContractRepository;
        private bool disposedValue;

        public ISmartContractRepository SmartContractRepository
        {
            get
            {
                return _smartContractRepository ??= new SmartContractRepository(_msSqlContext, _distributedCache, this);
            }
        }

        public UnitOfWorkRepository(MsSqlDbContext msSqlContext, IDistributedCache distributedCache)
        {
            _msSqlContext = msSqlContext;
            _distributedCache = distributedCache;
        }

        public async Task SaveChangesAsync()
        {
            await _msSqlContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

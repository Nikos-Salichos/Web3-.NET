﻿using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        public MsqlDbContext _msqlSqlContext;
        public ISmartContractRepository _smartContractRepository;
        private bool disposedValue;

        public ISmartContractRepository SmartContractRepository
        {
            get
            {
                return _smartContractRepository ??= new SmartContractRepository(_msqlSqlContext);
            }
        }

        public UnitOfWorkRepository(MsqlDbContext repositoryContext)
        {
            _msqlSqlContext = repositoryContext;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _msqlSqlContext.SaveChangesAsync() > 0;
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

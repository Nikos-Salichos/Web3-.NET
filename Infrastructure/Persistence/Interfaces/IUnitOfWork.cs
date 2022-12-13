namespace Infrastructure.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISmartContractRepository SmartContractRepository { get; }
        Task<bool> SaveChangesAsync();
    }
}

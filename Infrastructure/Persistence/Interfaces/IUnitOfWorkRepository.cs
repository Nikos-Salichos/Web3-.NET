namespace Infrastructure.Persistence.Interfaces
{
    public interface IUnitOfWorkRepository : IDisposable
    {
        ISmartContractRepository SmartContractRepository { get; }
        Task<int> SaveChangesAsync();
    }
}

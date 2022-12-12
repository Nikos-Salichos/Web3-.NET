namespace Infrastructure.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITokenRepository Tokens { get; }
        int Save();
    }
}

namespace Infrastructure.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ILotteryRepository LotteryRepository { get; }
        Task<bool> SaveChanges();
    }
}

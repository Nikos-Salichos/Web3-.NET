using Domain.Models;

namespace Infrastructure.Persistence.Interfaces
{
    public interface ILotteryRepository : IGenericRepository<Token>
    {
        Task Enter();
        Task GetRandomNumber();
        Task GetBalance();
        Task GetPlayers();
        Task GetWinnerByLottery();
        Task PickWinner();
    }
}

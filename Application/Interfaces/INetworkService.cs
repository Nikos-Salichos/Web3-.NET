using Nethereum.RPC.Eth.DTOs;

namespace Application.Interfaces
{
    public interface INetworkService
    {
        Task<BlockWithTransactionHashes> GetLatestBlockAsync();
        Task<BlockWithTransactionHashes[]> GetTransactionsOfABlock();
    }
}

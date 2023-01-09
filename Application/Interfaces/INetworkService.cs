using Nethereum.RPC.Eth.DTOs;

namespace Application.Interfaces
{
    public interface INetworkService
    {
        Task<BlockWithTransactionHashes> GetBlockAsync();
        Task<Transaction[]> GetTransactionsOfABlock();
        Task<IEnumerable<Transaction>> GetAllContractCreationTransactionsAsync();
    }
}

using Application.Interfaces;
using Nethereum.RPC.Eth.DTOs;

namespace Application.Services
{
    public class NetworkService : INetworkService
    {
        public Task<IEnumerable<Transaction>> GetAllContractCreationTransactionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlockWithTransactionHashes> GetLatestBlockAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Transaction[]> GetTransactionsOfABlock()
        {
            throw new NotImplementedException();
        }
    }
}

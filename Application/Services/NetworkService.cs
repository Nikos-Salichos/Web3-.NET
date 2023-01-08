using Application.Interfaces;
using Application.Utilities;
using Nethereum.RPC.Eth.DTOs;

namespace Application.Services
{
    public class NetworkService : INetworkService
    {
        public EnumHelper EnumHelper { get; set; }

        private readonly ISingletonOptionsService _singletonOptionsService;

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

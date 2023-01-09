using Application.Interfaces;
using Application.Utilities;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Application.Services
{
    public class NetworkService : INetworkService
    {
        public EnumHelper EnumHelper { get; set; }

        private readonly ISingletonOptionsService _singletonOptionsService;

        public NetworkService(EnumHelper enumHelper, ISingletonOptionsService singletonOptionsService)
        {
            EnumHelper = enumHelper;
            _singletonOptionsService = singletonOptionsService;
        }

        public Task<IEnumerable<Transaction>> GetAllContractCreationTransactionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlockWithTransactionHashes> GetBlockAsync()
        {
            Account? account = new(_user.PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

            HexBigInteger? latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            BlockWithTransactionHashes? latestBlock = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(latestBlockNumber);

            if (latestBlock == null)
            {
                return NotFound("Block not found");
            }

            return Ok($"Last block number {latestBlockNumber}, latest block gas limit {latestBlock.GasLimit}, latest block gas used {latestBlock.GasUsed}");
        }

        public Task<Transaction[]> GetTransactionsOfABlock()
        {
            throw new NotImplementedException();
        }
    }
}

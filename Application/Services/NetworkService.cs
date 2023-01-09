using Application.Interfaces;
using Application.Utilities;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;

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

        public async Task<BlockWithTransactionHashes> GetBlockAsync(BigInteger blockNumber, Chain chain)
        {
            Account? account = new(_singletonOptionsService.GetUserSettings().PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

            BlockWithTransactionHashes? blockWithTransactionHashes = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new HexBigInteger(blockNumber));

            return blockWithTransactionHashes ?? new BlockWithTransactionHashes();
        }

        public Task<Transaction[]> GetTransactionsOfABlock()
        {
            throw new NotImplementedException();
        }
    }
}

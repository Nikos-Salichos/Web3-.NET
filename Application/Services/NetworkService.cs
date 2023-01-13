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

        public NetworkService(ISingletonOptionsService singletonOptionsService)
        {
            EnumHelper = new EnumHelper(singletonOptionsService!);
            _singletonOptionsService = singletonOptionsService;
        }

        public async Task<BlockWithTransactionHashes> GetBlockAsync(BigInteger blockNumber, Chain chain)
        {
            Account? account = new(_singletonOptionsService.GetUserSettings().PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

            BlockWithTransactionHashes? blockWithTransactionHashes = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new HexBigInteger(blockNumber));

            return blockWithTransactionHashes ?? new BlockWithTransactionHashes();
        }

        public async Task<IEnumerable<Transaction?>> GetAllContractCreationTransactionsAsync(BigInteger blockNumber, Chain chain)
        {
            Account? account = new(_singletonOptionsService.GetUserSettings().PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

            BlockWithTransactions? blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(blockNumber));

            if (blockWithTransactions == null)
            {
                return Array.Empty<Transaction>();
            }

            return blockWithTransactions.Transactions.Where(t => t.To == null);
        }

        public async Task<Transaction[]> GetTransactionsOfABlock(BigInteger blockNumber, Chain chain)
        {
            Account? account = new(_singletonOptionsService.GetUserSettings().PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));
            BlockWithTransactions? blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(blockNumber));

            if (blockWithTransactions == null)
            {
                return Array.Empty<Transaction>();
            }

            return blockWithTransactions.Transactions ?? Array.Empty<Transaction>();
        }
    }
}

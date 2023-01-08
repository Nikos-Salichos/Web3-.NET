using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Reflection;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly User _user = new();

        public EnumHelper EnumHelper { get; set; }

        private readonly ILogger<LotteryController> _logger;

        public NetworkController(IConfiguration configuration, ILogger<LotteryController> logger)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>()!;
            _logger = logger;
        }

        [HttpGet("GetLatestBlock")]
        public async Task<ActionResult> GetLatestBlockAsync(Chain chain)
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

        [HttpGet("GetAllTransactionsOfCurrentBlock")]
        public async Task<ActionResult> GetTransactionsOfABlockAsync(Chain chain)
        {
            Account? account = new(_user.PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

            HexBigInteger? latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            BlockWithTransactions? blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(latestBlockNumber));
            if (blockWithTransactions == null)
            {
                return NotFound("Block not found");
            }

            Transaction[] allTransactions = blockWithTransactions.Transactions;
            return Ok(allTransactions);
        }

        [HttpGet("GetAllTransactionsOfABlock")]
        public async Task<ActionResult> GetTransactionsOfABlock(Chain chain, long blockNumber)
        {
            Account? account = new(_user.PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

            BlockWithTransactions? blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(blockNumber));
            if (blockWithTransactions == null)
            {
                return NotFound("Block not found");
            }

            Transaction[] allTransactions = blockWithTransactions.Transactions;

            return Ok(allTransactions);
        }

        [HttpGet("GetAllContractCreationTransactions")]
        public async Task<ActionResult> GetAllContractCreationTransactions(Chain chain, long blockNumber)
        {
            Account? account = new(_user.PrivateKey, chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

            BlockWithTransactions? blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(blockNumber));

            if (blockWithTransactions == null)
            {
                _logger.LogError(MethodBase.GetCurrentMethod()?.Name + " Block not found");
                return NotFound("Block not found");
            }

            Transaction[] allTransactions = blockWithTransactions.Transactions;

            if (allTransactions.Length == 0)
            {
                _logger.LogError(MethodBase.GetCurrentMethod()?.Name + " Transactions not found");
                return NotFound("Transactions not found");
            }

            IEnumerable<Transaction> transactionsForContractCreation = allTransactions.Where(t => t.To == null);

            return Ok(transactionsForContractCreation);
        }
    }
}

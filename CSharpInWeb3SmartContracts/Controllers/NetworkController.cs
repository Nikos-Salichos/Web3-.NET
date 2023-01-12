using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
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

        private readonly INetworkService _networkService;

        public NetworkController(IConfiguration configuration, ILogger<LotteryController> logger, INetworkService networkService)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>()!;
            _logger = logger;
            _networkService = networkService;
        }

        [HttpGet("GetLatestBlock")]
        public async Task<IActionResult> GetBlockAsync(BigInteger blockNumber, Chain chain)
        {
            var block = await _networkService.GetBlockAsync(blockNumber, chain);
            return Ok(block);
        }

        [HttpGet("GetAllTransactionsOfABlock")]
        public async Task<IActionResult> GetTransactionsOfABlockAsync(long blockNumber, Chain chain)
        {
            var transactions = await _networkService.GetTransactionsOfABlock(blockNumber, chain);
            return Ok(transactions);
        }

        [HttpGet("GetAllContractCreationTransactions")]
        public async Task<ActionResult> GetAllContractCreationTransactionsAsync(Chain chain, long blockNumber)
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

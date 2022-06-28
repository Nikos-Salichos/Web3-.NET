using CSharpInWeb3SmartContracts.Enumerations;
using CSharpInWeb3SmartContracts.Models;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {

        private readonly User _user = new User();

        public EnumHelper EnumHelper { get; set; }

        public NetworkController(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
            _user.BlockchainProviderKovan = configuration["BlockchainProviderKovan"];
            _user.MetamaskAddress = configuration["MetamaskAddress"];
            _user.PrivateKey = configuration["PrivateKey"];
        }

        [HttpGet("GetLatestBlock")]
        public async Task<ActionResult> GetLatestBlock(Chain chain, BlockchainNetworks blockchainNetwork)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(blockchainNetwork));

            HexBigInteger? latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            BlockWithTransactionHashes? latestBlock = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(latestBlockNumber);

            return Ok($"Last block number {latestBlockNumber}, latest block gas limit {latestBlock.GasLimit}, latest block gas used {latestBlock.GasUsed}");
        }


        [HttpGet("GetAllTransactionOfABlock")]
        public async Task<ActionResult> GetBalance(Chain chain, BlockchainNetworks blockchainNetwork)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(blockchainNetwork));

                HexBigInteger? latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

                BlockWithTransactions? blockWithTransactions = (await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(latestBlockNumber)));

                List<Transaction>? allTransactions = blockWithTransactions.Transactions.ToList();
                return Ok(allTransactions);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);

            }
        }

    }
}

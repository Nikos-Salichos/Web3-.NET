using CSharpInWeb3SmartContracts.Models;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;

        private readonly User _user = new User();
        public EnumHelper EnumHelper { get; set; }

        public WalletController(IConfiguration configuration, ILogger<WalletController> logger)
        {
            EnumHelper = new EnumHelper(configuration);
            _user.MetamaskAddress = configuration["MetamaskAddress"];
            _user.PrivateKey = configuration["PrivateKey"];
            _logger = logger;
        }

        [HttpPost("GetBalance")]
        public async Task<ActionResult> GetBalance(Chain chain)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                HexBigInteger? balance = await web3.Eth.GetBalance.SendRequestAsync(_user.MetamaskAddress);

                BigDecimal etherAmount = Web3.Convert.FromWeiToBigDecimal(balance.Value);

                return Ok($"Ethereum balance {etherAmount}");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("SendEthereum")]
        public async Task<ActionResult> SendEthereum(Chain chain, string toAddress, decimal etherAmount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                TransactionReceipt? transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(toAddress, etherAmount);

                return Ok($"Transaction hash {transaction.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("TransferTokens")]
        public async Task<ActionResult> SendERC20Token(Chain chain, string toAddress, long amountOfTokens)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                TransferFunction? transferFunction = new TransferFunction()
                {
                    FromAddress = account.Address,
                    To = toAddress,
                    Gas = 50000,
                    Value = amountOfTokens, //value of transfer tokens
                };

                IContractTransactionHandler<TransferFunction> transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
                TransactionReceipt? transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync("0xd0a1e359811322d97991e03f863a0c30c2cf029c", transferFunction);
                return Ok(transactionReceipt.TransactionHash);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);

            }
        }

        [HttpGet("PendingTransactions")]
        public async Task<ActionResult> GetPendingTransactions(Chain chain)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                HexBigInteger? pendingFilter = await web3.Eth.Filters.NewPendingTransactionFilter.SendRequestAsync();
                string[]? filterChanges = await web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(pendingFilter);

                return Ok(filterChanges);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);

            }
        }

        /*                TransactionInput? transactionInput = new TransactionInput();
                           transactionInput.From = account.Address;
                           transactionInput.GasPrice = new HexBigInteger(new BigInteger(2));
                           transactionInput.To = _smartContractAddress;
                           transactionInput.Value = new HexBigInteger(new BigInteger(2));
                           var transactionHash = await web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);*/

    }
}

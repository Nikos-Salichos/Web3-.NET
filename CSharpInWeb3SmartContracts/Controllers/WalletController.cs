using CSharpInWeb3SmartContracts.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly User _user = new User();

        public WalletController(IConfiguration configuration)
        {
            _user.BlockchainProvider = configuration["BlockchainProviderKovan"];
            _user.MetamaskAddress = configuration["MetamaskAddress"];
            _user.PrivateKey = configuration["PrivateKey"];
        }

        [HttpGet("GetBalance")]
        public async Task<ActionResult> GetBalance(Chain chain)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, _user.BlockchainProvider);

            HexBigInteger? balance = await web3.Eth.GetBalance.SendRequestAsync(_user.MetamaskAddress);
            decimal etherAmount = Web3.Convert.FromWei(balance.Value);

            return Ok($"Ethereum balance {etherAmount}");
        }

        [HttpGet("SendEthereum")]
        public async Task<ActionResult> SendEthereum(Chain chain, string toAddress, decimal etherAmount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, _user.BlockchainProvider);

                TransactionReceipt? transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(toAddress, etherAmount);

                return Ok($"Transaction hash {transaction.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);

            }
        }


        [HttpGet("TransferTokens")]
        public async Task<ActionResult> SendERC20Token() //Chain chain,string toAddress, string contractAddress, 
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, Chain.Kovan);
                Web3? web3 = new Web3(account, _user.BlockchainProvider);

                TransferFunction? transferFunction = new TransferFunction()
                {
                    FromAddress = account.Address,
                    To = "0x67ed7a6183199Fc01a3F2Eb7bb0dF20F76016F12",
                    // AmountToSend = 10000000000000000,
                    Gas = 50000,
                    // GasPrice = 25000,
                    // MaxFeePerGas = 21000,
                    Value = 10000000000000000, //send amount of ERC20 tokens and NOT value in transaction
                };

                IContractTransactionHandler<TransferFunction> transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);

            }
        }



    }
}

using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoTokenController : ControllerBase
    {
        private readonly string _abi = @" [{""inputs"":[{""internalType"":""uint256"",""name"":""initialSupply"",""type"":""uint256""},{""internalType"":""string"",""name"":""tokenName"",""type"":""string""},{""internalType"":""string"",""name"":""tokenSymbol"",""type"":""string""},{""internalType"":""uint256"",""name"":""tokenCap"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":false,""internalType"":""address"",""name"":""from"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""Burn"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""DecreaseApproval"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""IncreaseApproval"",""type"":""event""},{""anonymous"":false,""inputs"":[],""name"":""NotPausable"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""previousOwner"",""type"":""address""}],""name"":""OwnershipRenounced"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""previousOwner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""newOwner"",""type"":""address""}],""name"":""OwnershipTransferred"",""type"":""event""},{""anonymous"":false,""inputs"":[],""name"":""Pause"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":false,""internalType"":""address"",""name"":""from"",""type"":""address""},{""indexed"":false,""internalType"":""address"",""name"":""to"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""Sent"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""from"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""to"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""},{""anonymous"":false,""inputs"":[],""name"":""Unpause"",""type"":""event""},{""inputs"":[{""internalType"":""address"",""name"":""owner"",""type"":""address""},{""internalType"":""address"",""name"":""delegate"",""type"":""address""}],""name"":""allowance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""},{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""allowed"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""delegate"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""burn"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""from"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""burnFrom"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""canPause"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""cap"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""decimals"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""spender"",""type"":""address""},{""internalType"":""uint256"",""name"":""subtractedAmount"",""type"":""uint256""}],""name"":""decreaseApproval"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address payable"",""name"":""_to"",""type"":""address""}],""name"":""destroySmartContract"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenOwner"",""type"":""address""}],""name"":""getBalance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""spender"",""type"":""address""},{""internalType"":""uint256"",""name"":""addedAmount"",""type"":""uint256""}],""name"":""increaseApproval"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""receiver"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""mint"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""name"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""notPausable"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""owner"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""pause"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""paused"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""renounceOwnership"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""symbol"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""totalSupply"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""receiver"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""from"",""type"":""address""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""newOwner"",""type"":""address""}],""name"":""transferOwnership"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""unpause"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""}] ";

        private readonly string _smartContractAddress = "0x0d26f523c24feda020c293185ac7a032814aedcf";

        private readonly WalletOwner _user = new();

        public EnumHelper EnumHelper { get; set; }

        public CryptoTokenController(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<WalletOwner>();
        }

        [HttpGet("GetBalance")]
        public async Task<ActionResult> GetBalance(Chain chain, string address)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[1] { address };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? getBalance = smartContract.GetFunction("getBalance");

            BigInteger getBalanceResult = await getBalance.CallAsync<BigInteger>(parameters);

            BigDecimal balanceInEth = Web3.Convert.FromWeiToBigDecimal(getBalanceResult);

            return Ok($"{address} has {balanceInEth} tokens");
        }

        [HttpGet("Mint")]
        public async Task<ActionResult> Mint(Chain chain, string receiverAddress, long amount)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[2] { receiverAddress, amount };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? mint = smartContract.GetFunction("mint");

            HexBigInteger? estimatedGas = await mint.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? mintResult = await mint.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok();
        }

        [HttpGet("Approve")]
        public async Task<ActionResult> Approve(Chain chain, string spenderAddress, long amount)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[2] { spenderAddress, amount };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? approve = smartContract.GetFunction("approve");

            HexBigInteger? estimatedGas = await approve.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? approveResult = await approve.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok();
        }

        [HttpGet("Allowance")]
        public async Task<ActionResult> Allowance(Chain chain, string ownerAddress, string spenderAddress)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[2] { ownerAddress, spenderAddress };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? allowance = smartContract.GetFunction("allowance");

            BigInteger allowanceAmount = await allowance.CallAsync<BigInteger>(parameters);
            BigDecimal balanceInEth = Web3.Convert.FromWeiToBigDecimal(allowanceAmount);

            return Ok($"{spenderAddress} allow to spend {balanceInEth} ETH of {ownerAddress}");
        }

        [HttpGet("DestroySmartContract")]
        public async Task<ActionResult> DestroySmartContract(Chain chain, string withdrawalAddress)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[1] { withdrawalAddress };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? destroySmartContract = smartContract.GetFunction("destroySmartContract");

            HexBigInteger? estimatedGas = await destroySmartContract.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? destroySmartContractResult = await destroySmartContract.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok($"Smart contract destroyed {destroySmartContractResult.TransactionHash}");
        }

        [HttpGet("Transfer")]
        public async Task<ActionResult> Transfer(Chain chain, string receiver, long amountOfTokens)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[2] { receiver, amountOfTokens };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? transfer = smartContract.GetFunction("transfer");

            TransferFunction? transferFunction = new TransferFunction()
            {
                FromAddress = account.Address,
                To = receiver,
                Gas = 50000,
                Value = amountOfTokens,
            };

            HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok($"Tokens transfer was successful {transferResult.TransactionHash}");
        }

        [HttpGet("IncreaseApproval")]
        public async Task<ActionResult> IncreaseApproval(Chain chain, string spender, long addedAmount)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[2] { spender, addedAmount };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? transfer = smartContract.GetFunction("increaseApproval");

            HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);
            return Ok($"Increase approval was successful {transferResult.TransactionHash}");
        }

        [HttpGet("DecreaseApproval")]
        public async Task<ActionResult> DecreaseApproval(Chain chain, string spender, long subtractedAmount)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[2] { spender, subtractedAmount };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? transfer = smartContract.GetFunction("decreaseApproval");

            HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok($"Decrease approval was successful {transferResult.TransactionHash}");
        }

        [HttpGet("Burn")]
        public async Task<ActionResult> Burn(Chain chain, long amount)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[1] { amount };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? transfer = smartContract.GetFunction("burn");

            HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok($"{amount} tokens was burned {transferResult.TransactionHash}");
        }

        [HttpGet("TransferFrom")]
        public async Task<ActionResult> TransferFrom(Chain chain, string from, string to, long amount)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[3] { from, to, amount };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? transfer = smartContract.GetFunction("transferFrom");

            HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok($"Tokens transfer from {from} to {to} by {account.Address} was successful {transferResult.TransactionHash}");
        }

        [HttpGet("BurnFrom")]
        public async Task<ActionResult> BurnFrom(Chain chain, string from, long amount)
        {
            Account? account = new Account(_user.PrivateKey, chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

            object[]? parameters = new object[2] { from, amount };
            Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
            Function? burnFrom = smartContract.GetFunction("burnFrom");

            HexBigInteger? estimatedGas = await burnFrom.EstimateGasAsync(account.Address, null, null, parameters);

            TransactionReceipt? transferResult = await burnFrom.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

            return Ok($"Tokens burned from {from} successfully {transferResult.TransactionHash}");
        }
    }
}

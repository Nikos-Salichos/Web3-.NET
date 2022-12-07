using CSharpInWeb3SmartContracts.Models;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotteryController : ControllerBase
    {
        private readonly string _abi = @" [{""inputs"":[{""internalType"":""address"",""name"":""oracleAddress"",""type"":""address""}],""stateMutability"":""nonpayable"",""type"":""constructor""},{""inputs"":[],""name"":""enter"",""outputs"":[],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[],""name"":""getBalance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""getPlayers"",""outputs"":[{""internalType"":""address payable[]"",""name"":"""",""type"":""address[]""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""getRandomNumber"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""historyLotteryId"",""type"":""uint256""}],""name"":""getWinnerByLottery"",""outputs"":[{""internalType"":""address payable"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""name"":""lotteryHistory"",""outputs"":[{""internalType"":""address payable"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""lotteryId"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""owner"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""pickWinner"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""name"":""players"",""outputs"":[{""internalType"":""address payable"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""}] ";

        private readonly string _byteCode = "608060405234801561001057600080fd5b506040516106b63803806106b683398101604081905261002f91610060565b600080546001600160a01b039092166001600160a01b03199283161790556001805490911633178155600355610090565b60006020828403121561007257600080fd5b81516001600160a01b038116811461008957600080fd5b9392505050565b6106178061009f6000396000f3fe6080604052600436106100915760003560e01c80638da5cb5b116100595780638da5cb5b14610175578063dbdff2c114610195578063e580f47b146101aa578063e97dcb62146101c0578063f71d96cb146101c857600080fd5b806312065fe014610096578063281d098d146100b85780635d495aea146101065780636d6fe2301461011d5780638b5b9ccc14610153575b600080fd5b3480156100a257600080fd5b50475b6040519081526020015b60405180910390f35b3480156100c457600080fd5b506100ee6100d336600461051c565b6000908152600460205260409020546001600160a01b031690565b6040516001600160a01b0390911681526020016100af565b34801561011257600080fd5b5061011b6101e8565b005b34801561012957600080fd5b506100ee61013836600461051c565b6004602052600090815260409020546001600160a01b031681565b34801561015f57600080fd5b506101686102ed565b6040516100af9190610535565b34801561018157600080fd5b506001546100ee906001600160a01b031681565b3480156101a157600080fd5b506100a561034f565b3480156101b657600080fd5b506100a560035481565b61011b6103c6565b3480156101d457600080fd5b506100ee6101e336600461051c565b610478565b6001546001600160a01b031633146101ff57600080fd5b60025460009061020d61034f565b6102179190610582565b90506002818154811061022c5761022c6105a4565b60009182526020822001546040516001600160a01b03909116914780156108fc02929091818181858888f1935050505015801561026d573d6000803e3d6000fd5b5060028181548110610281576102816105a4565b600091825260208083209091015460038054845260049092526040832080546001600160a01b0319166001600160a01b039092169190911790558054916102c7836105ba565b909155505060408051600081526020810191829052516102e9916002916104a2565b5050565b6060600280548060200260200160405190810160405280929190818152602001828054801561034557602002820191906000526020600020905b81546001600160a01b03168152600190910190602001808311610327575b5050505050905090565b600154600080546040805160609490941b6bffffffffffffffffffffffff191660208086019190915242603486015263604a6fa992901b640100000000600160c01b031691909117901b605483015244606c83015290608c016040516020818303038152906040528051906020012060001c905090565b662386f26fc1000034116104345760405162461bcd60e51b815260206004820152602b60248201527f4d696e696d756d20616d6f756e7420746f20706172746963697061746520697360448201526a10181718189032ba3432b960a91b606482015260840160405180910390fd5b600280546001810182556000919091527f405787fa12a823e0f2b7631cc41b3ba8828b3321ca811111fa75cd3aa3bb5ace0180546001600160a01b03191633179055565b6002818154811061048857600080fd5b6000918252602090912001546001600160a01b0316905081565b8280548282559060005260206000209081019282156104f7579160200282015b828111156104f757825182546001600160a01b0319166001600160a01b039091161782556020909201916001909101906104c2565b50610503929150610507565b5090565b5b808211156105035760008155600101610508565b60006020828403121561052e57600080fd5b5035919050565b6020808252825182820181905260009190848201906040850190845b818110156105765783516001600160a01b031683529284019291840191600101610551565b50909695505050505050565b60008261059f57634e487b7160e01b600052601260045260246000fd5b500690565b634e487b7160e01b600052603260045260246000fd5b6000600182016105da57634e487b7160e01b600052601160045260246000fd5b506001019056fea26469706673582212200bcfbeab68ab0564573a12cdad657c74a3c0d2b5a752fefac1d11059b128dd0f64736f6c634300080e0033";

        // private readonly string _smartContractAddress = "0xF321FcC68DB5755f81766cc6B631651bBB1E1cAD";

        private readonly User _user = new();

        public EnumHelper EnumHelper { get; set; }

        public LotteryController(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration?.GetSection("User").Get<User>();
        }

        [HttpGet("Deploy")]
        public async Task<ActionResult> DeployContract(Chain chain)
        {
            try
            {
                object[]? parametersForPair = new object[1] { _user.WalletAddress };

                Account? account = new(_user.PrivateKey, chain);
                Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

                HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(_abi,
                                                                                            _byteCode,
                                                                                            _user.WalletAddress,
                                                                                            parametersForPair);

                TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(_abi,
                                                                                                                     _byteCode,
                                                                                                                     _user.WalletAddress,
                                                                                                                     estimatedGas,
                                                                                                                     null,
                                                                                                                     parametersForPair);

                return Ok($"Deploy Contract Transaction Hash {deployContract.TransactionHash} , smart contract address {deployContract.ContractAddress}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetRandomNumber")]
        public async Task<ActionResult> GetRandomNumber(Chain chain, string smartContractAddress)
        {
            try
            {
                Account? account = new(_user.PrivateKey, chain);
                Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_abi, smartContractAddress);
                Function? getRandomNumber = smartContract.GetFunction("getRandomNumber");
                BigInteger getRandomNumberResult = await getRandomNumber.CallAsync<BigInteger>();

                return Ok($"Random Number {getRandomNumberResult}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetPlayers")]
        public async Task<ActionResult> GetPlayers(Chain chain, string smartContractAddress)
        {
            try
            {
                Account? account = new(_user.PrivateKey, chain);
                Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

                var smartContract = web3.Eth.GetContract(_abi, smartContractAddress);
                Function? getPlayers = smartContract.GetFunction("getPlayers");
                List<string> getPlayersResult = await getPlayers.CallAsync<List<string>>();

                return Ok(string.Join(",", getPlayersResult));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetBalance")]
        public async Task<ActionResult> GetBalance(Chain chain, string smartContractAddress)
        {
            try
            {
                Account? account = new(_user.PrivateKey, chain);
                Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_abi, smartContractAddress);
                Function? getBalance = smartContract.GetFunction("getBalance");
                long getBalanceResult = await getBalance.CallAsync<long>();

                BigDecimal balanceInEth = Web3.Convert.FromWeiToBigDecimal(getBalanceResult);

                return Ok($"Smart contract balance {balanceInEth}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("EnterLottery")]
        public async Task<ActionResult> EnterLottery(Chain chain, string smartContractAddress)
        {
            try
            {
                Account? account = new(_user.PrivateKey, chain);
                Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_abi, smartContractAddress);

                BigInteger wei = Web3.Convert.ToWei(0.02);
                HexBigInteger value = new(wei);
                HexBigInteger gas = new(wei);

                Function? enterLottery = smartContract.GetFunction("enter");
                HexBigInteger? estimatedGas = await enterLottery.EstimateGasAsync(account.Address, gas, value, null);
                TransactionReceipt? enterLotteryResult = await enterLottery.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, value, null);

                return Ok($"Transaction Hash {enterLotteryResult.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("PickWinner")]
        public async Task<ActionResult> PickWinner(Chain chain, string smartContractAddress)
        {
            try
            {
                Account? account = new(_user.PrivateKey, chain);
                Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_abi, smartContractAddress);

                Function? pickWinner = smartContract.GetFunction("pickWinner");
                HexBigInteger? estimatedGas = await pickWinner.EstimateGasAsync(account.Address, null, null, null);
                TransactionReceipt? enterLotteryResult = await pickWinner.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null);

                return Ok($"Transaction Hash {enterLotteryResult.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}

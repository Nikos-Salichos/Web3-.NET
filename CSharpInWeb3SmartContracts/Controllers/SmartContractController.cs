using CSharpInWeb3SmartContracts.DTOs;
using CSharpInWeb3SmartContracts.Models;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;

namespace CSharpInWeb3SmartContracts.Controllers
{
    public class SmartContractController : ControllerBase
    {
        private readonly User _user = new User();
        public EnumHelper EnumHelper { get; set; }

        public SmartContractController(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>();
        }

        [Consumes("application/json")]
        [HttpPost("DeployAnyContract")]
        public async Task<ActionResult> DeployContract(Chain chain, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = null;
                if (smartContractModel?.Parameters?.Count > 0)
                {
                    parameters = smartContractModel.Parameters.ToArray();
                    if (string.IsNullOrWhiteSpace(parameters?.FirstOrDefault()?.ToString()))
                    {
                        parameters = null;
                    }
                }

                HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(smartContractModel?.Abi.ToString(),
                                                                                          smartContractModel?.Bytecode,
                                                                                          _user.WalletAddress,
                                                                                          parameters);

                TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(smartContractModel?.Abi.ToString(),
                                                                                                                    smartContractModel?.Bytecode,
                                                                                                                    _user.WalletAddress,
                                                                                                                    estimatedGas,
                                                                                                                    null, null, null, parameters);

                return Ok(deployContract);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Consumes("application/json")]
        [HttpPost("CallContractVariable")]
        public async Task<ActionResult> CallContractVariable(Chain chain, string variableName, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(smartContractModel.Abi.ToString(), smartContractModel.Address);
                Function? variable = smartContract.GetFunction(variableName);

                dynamic variableValue = await variable.CallAsync<dynamic>();

                if (variableValue is null)
                {
                    return Ok(variableName + "is null");
                }

                return Ok(variableName + ": " + variableValue.ToString());
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Consumes("application/json")]
        [HttpPost("CallReadFunction")]
        public async Task<ActionResult> CallReadFunction(Chain chain, string variableName, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(smartContractModel.Abi.ToString(), smartContractModel.Address);

                Function? readFunction = smartContract.GetFunction(variableName);
                object[]? parameters = null;

                if (smartContractModel?.Parameters?.Count > 0)
                {
                    parameters = smartContractModel.Parameters.ToArray();
                }

                dynamic variableValue = await readFunction.CallAsync<dynamic>(parameters);

                return Ok(variableName + ": " + variableValue.ToString());
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Consumes("application/json")]
        [HttpPost("CallWriteFunction")]
        public async Task<ActionResult> CallWriteFunction(Chain chain, string functionName, long sendAsEth, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(smartContractModel.Abi.ToString(), smartContractModel.Address);
                Function? writeFunction = smartContract.GetFunction(functionName);
                object[]? parameters = null;

                if (smartContractModel?.Parameters?.Count > 0)
                {
                    parameters = smartContractModel.Parameters.ToArray();
                    if (string.IsNullOrWhiteSpace(parameters?.FirstOrDefault()?.ToString()))
                    {
                        parameters = null;
                    }
                }

                HexBigInteger? value = null;
                BigInteger wei = Web3.Convert.ToWei(sendAsEth);
                if (wei != 0)
                {
                    value = new HexBigInteger(wei);
                }

                HexBigInteger? estimatedGas = await writeFunction.EstimateGasAsync(account.Address, null, value, parameters);
                TransactionReceipt? functionResult = await writeFunction.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, value, null, parameters);
                return Ok(functionResult);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Consumes("application/json")]
        [HttpPost("TrackCryptoWhalesForAnyToken")]
        public async Task<ActionResult> TrackCryptoWhalesForAnyToken(Chain chain, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));
                Contract? smartContract = web3.Eth.GetContract(smartContractModel.Abi.ToString(), smartContractModel.Address);

                Event transferEvent = smartContract.GetEvent("Transfer");
                BlockParameter? _lastBlock = BlockParameter.CreateLatest();
                NewFilterInput? filterInput = transferEvent.CreateFilterInput(_lastBlock, _lastBlock);
                List<EventLog<TransferEventDTO>>? transfers = await transferEvent.GetAllChangesAsync<TransferEventDTO>(filterInput);

                return Ok(transfers);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Consumes("application/json")]
        [HttpPost("DeployInMultipleChains")]
        public async Task<ActionResult> DeployInMultipleChains(List<Chain> chains, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                List<TransactionReceipt> transactionReceipts = new List<TransactionReceipt>();
                foreach (var chain in chains)
                {
                }
                return null;
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}

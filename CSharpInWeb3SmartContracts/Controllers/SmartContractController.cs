using CSharpInWeb3SmartContracts.Models;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

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
        public async Task<ActionResult> DeployContract(Chain chain, User user, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(user.PrivateKey, chain);
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
                                                                                          user.WalletAddress,
                                                                                          parameters);

                TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(smartContractModel?.Abi.ToString(),
                                                                                                                    smartContractModel?.Bytecode,
                                                                                                                    user.WalletAddress,
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
        public async Task<ActionResult> CallContractVariable(Chain chain, string privateKey, string variableName, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(privateKey, chain);
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
        public async Task<ActionResult> CallReadFunction(Chain chain, string privateKey, string variableName, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(privateKey, chain);
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
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }




    }




}

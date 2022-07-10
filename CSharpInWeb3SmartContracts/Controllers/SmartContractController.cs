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
        public EnumHelper EnumHelper { get; set; }

        public SmartContractController(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
        }

        [Consumes("application/json")]
        [HttpPost("DeployWithoutParameters")]
        public async Task<ActionResult> DeployWithoutParameters(Chain chain, string privateKey, string metamaskAddress, string byteCode, [FromBody] object abi)
        {
            try
            {
                Account? account = new Account(privateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(abi.ToString(),
                                                                                            byteCode,
                                                                                            metamaskAddress,
                                                                                            null);

                TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(abi.ToString(),
                                                                                                                    byteCode,
                                                                                                                    metamaskAddress,
                                                                                                                    estimatedGas,
                                                                                                                    null, null, null, null);

                return Ok(deployContract);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Consumes("application/json")]
        [HttpPost("DeployWithParameters")]
        public async Task<ActionResult> DeployWithParameters(Chain chain, string privateKey, string metamaskAddress, [FromBody] SmartContractDeploy smartContractModel)
        {
            try
            {
                Account? account = new Account(privateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = smartContractModel.Parameters?.ToArray();

                HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(smartContractModel.Abi.ToString(),
                                                                                          smartContractModel.Bytecode,
                                                                                          metamaskAddress,
                                                                                          parameters);

                TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(smartContractModel.Abi.ToString(),
                                                                                                                    smartContractModel.Bytecode,
                                                                                                                    metamaskAddress,
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
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}

using Application.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    public class SmartContractController : ControllerBase
    {
        private readonly User _user = new User();
        public EnumHelper EnumHelper { get; set; }

        private readonly ISmartContractService _smartContractService;

        public SmartContractController(IConfiguration configuration, ISmartContractService smartContractService)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>()!;
            _smartContractService = smartContractService;
        }

        [HttpGet("GetAllSmartContracts")]
        [ProducesResponseType(typeof(IEnumerable<SmartContractDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllSmartContractsAsync()
        {
            var allSmartContracts = await _smartContractService.GetSmartContractsAsync();
            return Ok(allSmartContracts);
        }

        [Consumes("application/json")]
        [HttpPost("DeployAnyContract")]
        public async Task<ActionResult> DeployContractSuffix([FromBody] SmartContractDTO smartContractDto)
        {
            var deployedSmartContract = await _smartContractService.DeploySmartContractAsync(smartContractDto);
            return Ok(deployedSmartContract);
        }

        [Consumes("application/json")]
        [HttpPost("CallContractVariable")]
        public async Task<ActionResult> CallContractVariableAsync(string variableName, [FromBody] SmartContract smartContractModel)
        {
            var variableResult = await _smartContractService.ReadContractFunctionVariableAsync(variableName, smartContractModel);
            return Ok(variableName + ": " + variableResult.ToString());
        }

        [Consumes("application/json")]
        [HttpPost("CallReadFunction")]
        public async Task<ActionResult> ReadContractFunctionAsync(string variableName, [FromBody] SmartContract smartContractModel)
        {
            var variableResult = await _smartContractService.ReadContractFunctionVariableAsync(variableName, smartContractModel);
            return Ok(variableName + ": " + variableResult.ToString());
        }

        [Consumes("application/json")]
        [HttpPost("CallWriteFunction")]
        public async Task<ActionResult> CallWriteFunctionAsync(string functionName, long sendAsEth, [FromBody] SmartContract smartContractModel)
        {
            var functionResult = await _smartContractService.WriteContractFunctionVariableAsync(functionName, sendAsEth, smartContractModel);
            return Ok(functionName + ": " + functionResult.ToString());
        }

        [Consumes("application/json")]
        [HttpPost("TrackAnyEvent")]
        public async Task<ActionResult> TrackEventAsync(string eventName, SmartContract smartContractJson)
        {
            var eventResults = await _smartContractService.TrackEventAsync(eventName, smartContractJson);
            return eventResults.ToJson();
        }

        [Consumes("application/json")]
        [HttpPost("DeployInMultipleChains")]
        public async Task<ActionResult> DeployInMultipleChains(List<Chain> chains, [FromBody] SmartContract smartContractModel)
        {
            List<TransactionReceipt> transactionReceipts = new List<TransactionReceipt>();
            foreach (var chain in chains)
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

                HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(smartContractModel?.Abi?.ToString(),
                                                                                          smartContractModel?.Bytecode,
                                                                                          _user.WalletAddress,
                                                                                          parameters);

                TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(smartContractModel?.Abi?.ToString(),
                                                                                                                    smartContractModel?.Bytecode,
                                                                                                                    _user.WalletAddress,
                                                                                                                    estimatedGas,
                                                                                                                    null, null, null, parameters);

                if (deployContract != null)
                {
                    transactionReceipts.Add(deployContract);
                }
            }

            return Ok(string.Join(",", transactionReceipts));
        }

    }
}

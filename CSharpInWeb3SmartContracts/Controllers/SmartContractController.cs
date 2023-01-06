using Application.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    public class SmartContractController : ControllerBase
    {
        private readonly User _user = new User();
        public EnumHelper EnumHelper { get; set; }

        private readonly ISmartContractService _smartContractService;
        private readonly ILogger<LotteryController> _logger;

        public SmartContractController(IConfiguration configuration, ISmartContractService smartContractService, ILogger<LotteryController> logger)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>()!;
            _smartContractService = smartContractService;
            _logger = logger;
        }

        [HttpGet("GetAllSmartContracts")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSmartContractsAsync()
        {
            var allSmartContracts = await _smartContractService.GetSmartContractsAsync();
            _logger.LogInformation(allSmartContracts.ToString());
            return Ok(allSmartContracts);
        }

        [HttpGet("GetSmartContractById")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSmartContractAsync(long id)
        {
            var smartContract = await _smartContractService.GetSmartContractByIdAsync(id);
            _logger.LogInformation(smartContract.ToString());
            return Ok(smartContract);
        }

        [HttpGet("FindSmartContractsByAddress")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindSmartContractsByAddressAsync(string address)
        {
            var allSmartContracts = await _smartContractService.FindSmartContractsByAddressAsync(s => s.Address == address);
            _logger.LogInformation(allSmartContracts.ToString());
            return Ok(allSmartContracts);
        }

        [Consumes("application/json")]
        [HttpPost("DeployAnyContract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeployContractAsync([FromBody] SmartContractDTO smartContractDto)
        {
            var deployedSmartContract = await _smartContractService.DeploySmartContractAsync(smartContractDto);
            _logger.LogInformation(allSmartContracts.ToString());
            return Ok(deployedSmartContract);
        }

        [Consumes("application/json")]
        [HttpPost("CallContractVariable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CallContractVariableAsync(string variableName, [FromBody] SmartContract smartContractModel)
        {
            var variableResult = await _smartContractService.ReadContractFunctionVariableAsync(variableName, smartContractModel);
            return Ok(variableResult.ToString());
        }

        [Consumes("application/json")]
        [HttpPost("CallReadFunction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadContractFunctionAsync(string variableName, [FromBody] SmartContract smartContractModel)
        {
            var variableResult = await _smartContractService.ReadContractFunctionVariableAsync(variableName, smartContractModel);
            return Ok(variableResult.ToString());
        }

        [Consumes("application/json")]
        [HttpPost("CallWriteFunction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CallWriteFunctionAsync(string functionName, long sendAsEth, [FromBody] SmartContract smartContractModel)
        {
            var functionResult = await _smartContractService.WriteContractFunctionVariableAsync(functionName, sendAsEth, smartContractModel);
            return Ok(functionResult.ToString());
        }

        [Consumes("application/json")]
        [HttpPost("TrackAnyEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TrackEventAsync(string eventName, SmartContract smartContractJson)
        {
            var eventLogs = await _smartContractService.TrackEventAsync(eventName, smartContractJson);
            return Ok(eventLogs.ToString());
        }

    }
}

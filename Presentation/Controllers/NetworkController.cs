using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly ILogger<NetworkController> _logger;

        private readonly INetworkService _networkService;

        public NetworkController(ILogger<NetworkController> logger, INetworkService networkService)
        {
            _logger = logger;
            _networkService = networkService;
        }

        [HttpGet("GetBlockDetails")]
        public async Task<IActionResult> GetBlockAsync(long blockNumber, Chain chain)
        {
            var block = await _networkService.GetBlockAsync(blockNumber, chain);
            _logger.LogInformation("Block {@block}", block);
            return Ok(block);
        }

        [HttpGet("GetAllTransactionsOfABlock")]
        public async Task<IActionResult> GetTransactionsOfABlockAsync(long blockNumber, Chain chain)
        {
            var transactions = await _networkService.GetTransactionsOfABlock(blockNumber, chain);
            _logger.LogInformation("Transactions {@transactions}", transactions);
            return Ok(transactions);
        }

        [HttpGet("GetAllContractCreationTransactions")]
        public async Task<ActionResult> GetAllContractCreationTransactionsAsync(long blockNumber, Chain chain)
        {
            var creationContractTransactions = await _networkService.GetAllContractCreationTransactionsAsync(blockNumber, chain);

            return Ok(creationContractTransactions);
        }
    }
}

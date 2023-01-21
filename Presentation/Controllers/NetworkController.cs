using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly WalletOwner _user;

        public EnumHelper EnumHelper { get; set; }

        private readonly ILogger<NetworkController> _logger;

        private readonly INetworkService _networkService;

        public NetworkController(IConfiguration configuration, ILogger<NetworkController> logger, INetworkService networkService)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<WalletOwner>();
            _logger = logger;
            _networkService = networkService;
        }

        [HttpGet("GetBlockDetails")]
        public async Task<IActionResult> GetBlockAsync(long blockNumber, Chain chain)
        {
            var block = await _networkService.GetBlockAsync(blockNumber, chain);
            return Ok(block);
        }

        [HttpGet("GetAllTransactionsOfABlock")]
        public async Task<IActionResult> GetTransactionsOfABlockAsync(long blockNumber, Chain chain)
        {
            var transactions = await _networkService.GetTransactionsOfABlock(blockNumber, chain);
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

﻿using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer;
using System.Numerics;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly User _user = new();

        public EnumHelper EnumHelper { get; set; }

        private readonly ILogger<LotteryController> _logger;

        private readonly INetworkService _networkService;

        public NetworkController(IConfiguration configuration, ILogger<LotteryController> logger, INetworkService networkService)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>()!;
            _logger = logger;
            _networkService = networkService;
        }

        [HttpGet("GetLatestBlock")]
        public async Task<IActionResult> GetBlockAsync(BigInteger blockNumber, Chain chain)
        {
            var block = await _networkService.GetBlockAsync(blockNumber, chain);
            return Ok(block);
        }

        [HttpGet("GetAllTransactionsOfABlock")]
        public async Task<IActionResult> GetTransactionsOfABlockAsync(BigInteger blockNumber, Chain chain)
        {
            var transactions = await _networkService.GetTransactionsOfABlock(blockNumber, chain);
            return Ok(transactions);
        }

        [HttpGet("GetAllContractCreationTransactions")]
        public async Task<ActionResult> GetAllContractCreationTransactionsAsync(Chain chain, long blockNumber)
        {



            return Ok(transactionsForContractCreation);
        }
    }
}

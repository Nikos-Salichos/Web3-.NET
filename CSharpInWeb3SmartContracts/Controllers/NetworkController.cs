using CSharpInWeb3SmartContracts.Models;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {

        private readonly User _user = new User();

        public NetworkController(IConfiguration configuration)
        {
            _user.BlockchainProvider = configuration["BlockchainProvider"];
            _user.MetamaskAddress = configuration["MetamaskAddress"];
            _user.PrivateKey = configuration["PrivateKey"];
        }

        [HttpGet("GetLatestBlock")]
        public async Task<ActionResult> GetLatestBlock(Chain chain)
        {

        }

    }
}

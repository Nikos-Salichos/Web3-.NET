using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly User _user = new User();

        public WalletController(IConfiguration configuration)
        {
            _configuration = configuration;
            _user.BlockchainProvider = _configuration["BlockchainProvider"];
            _user.MetamaskAddress = _configuration["MetamaskAddress"];
            _user.PrivateKey = _configuration["PrivateKey"];
        }


        [HttpGet("GetBalance")]
        public async Task<ActionResult> GetBalance()
        {
            Account? account = new Account(_user.PrivateKey, Chain.Kovan);
            Web3? web3 = new Web3(account, _user.BlockchainProvider);

            HexBigInteger? balance = await web3.Eth.GetBalance.SendRequestAsync(_user.MetamaskAddress);



        }

    }
}

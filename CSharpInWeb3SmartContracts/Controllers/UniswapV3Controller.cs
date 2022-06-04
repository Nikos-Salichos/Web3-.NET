using CSharpInWeb3SmartContracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniswapV3Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly User _user = new User();


        public UniswapV3Controller(IConfiguration configuration)
        {
            _configuration = configuration;
            _user.BlockchainProvider = _configuration["BlockchainProvider"];
            _user.MetamaskAddress = _configuration["MetamaskAddress"];
            _user.PrivateKey = _configuration["PrivateKey"];
        }



    }
}

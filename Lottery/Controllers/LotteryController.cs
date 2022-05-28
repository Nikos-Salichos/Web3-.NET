using Microsoft.AspNetCore.Mvc;

namespace Lottery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotteryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly User _user = new User();

        public LotteryController(IConfiguration configuration)
        {
            _configuration = configuration;
            _user.BlockchainProvider = _configuration["BlockchainProvider"];
            _user.MetamaskAddress = _configuration["MetamaskAddress"];
            _user.PrivateKey = _configuration["PrivateKey"];
        }

        [HttpGet("AllHeroes")]
        public async Task<ActionResult> EnterLottery()
        {
            return NotFound();
        }
    }
}

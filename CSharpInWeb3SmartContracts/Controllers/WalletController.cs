using Microsoft.AspNetCore.Mvc;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly User _user = new User();

        public WalletController()
        {

        }
    }
}

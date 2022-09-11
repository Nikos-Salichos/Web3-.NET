using Microsoft.AspNetCore.Mvc;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoCompareController : ControllerBase
    {
        private string _apiKey { get; }

        public CryptoCompareController(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("CryptoCompare:APIKey").Get<string>();
        }
    }
}

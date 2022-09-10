using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinMarketCapController : ControllerBase
    {
        private string _apiKey { get; }

        public CoinMarketCapController(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("CoinMarketCap:APIKey").Get<string>();
        }

        [HttpGet("GetCoins")]
        public async Task<ActionResult> GetCoins()
        {
            try
            {
                var restClient = new RestClient("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

                var restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                restRequest.AddHeader("X-CMC_PRO_API_KEY", _apiKey);
            }
            catch (Exception exception)
            {

            }

        }
    }
}

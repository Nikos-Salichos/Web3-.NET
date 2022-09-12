using CSharpInWeb3SmartContracts.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

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

        [HttpGet("GetCoins")]
        public async Task<ActionResult> GetCoins()
        {
            try
            {
                List<Coin> coins = new List<Coin>();

                for (int i = 0; i < 2; i++)
                {
                    RestClient restClient = new RestClient($"https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&page={i}&tsym=USD&api_key={_apiKey}");

                    RestRequest restRequest = new RestRequest();


                }

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}

using CSharpInWeb3SmartContracts.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoCompareController : ControllerBase
    {
        private string? ApiKey { get; }

        public CryptoCompareController(IConfiguration? configuration)
        {
            ApiKey = configuration?.GetSection("CryptoCompare:APIKey").Get<string>();
        }

        [HttpGet("GetCoins")]
        public async Task<ActionResult> GetCoins()
        {
            try
            {
                List<Coin> coins = new();

                for (int i = 0; i < 2; i++)
                {
                    RestClient restClient = new($"https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&page={i}&tsym=USD&api_key={ApiKey}");

                    RestRequest restRequest = new();
                    restRequest.Method = Method.Get;

                    RestResponse response = await restClient.ExecuteAsync(restRequest, default);

                    if (response is null)
                    {
                        return NotFound("Response is null");
                    }

                    if (string.IsNullOrEmpty(response.Content))
                    {
                        return NotFound("No response content found");
                    }

                    CryptoCompare? cryptoCompare = JsonConvert.DeserializeObject<CryptoCompare>(response.Content);

                    if (cryptoCompare is null)
                    {
                        return NotFound("Response is empty");
                    }

                    if (cryptoCompare.Data is null)
                    {
                        return NotFound("Response Data is empty");
                    }

                    foreach (var cryptoCoin in cryptoCompare.Data)
                    {
                        Coin coin = new()
                        {
                            Id = cryptoCoin?.CoinInfo?.Id,
                            Name = cryptoCoin?.CoinInfo?.Name,
                            FullName = cryptoCoin?.CoinInfo?.FullName,
                            MarketCap = cryptoCoin?.RAW?.USD?.MKTCAP
                        };
                        coins.Add(coin);
                    }
                }

                List<Coin> sortedCoins = coins.OrderByDescending(c => c.MarketCap).ToList();

                return Ok(sortedCoins);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}

using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoCompareController : ControllerBase
    {
        private string? ApiKey { get; }
        private static readonly HttpClient client = new HttpClient();

        public CryptoCompareController(IConfiguration? configuration)
        {
            ApiKey = configuration?.GetSection("CryptoCompare:APIKey").Get<string>();
        }

        [HttpGet("GetCoins")]
        public async Task<ActionResult> GetCoins()
        {
            List<Token> coins = new();

            for (int i = 0; i < 2; i++)
            {
                var response = await client.GetAsync($"https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&page={i}&tsym=USD&api_key={ApiKey}");

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound("Request failed");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseContent))
                {
                    return NotFound("No response content found");
                }

                var cryptoCompare = JsonConvert.DeserializeObject<CryptoCompare>(responseContent);

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
                    var coin = new Token
                    {
                        Id = cryptoCoin?.CoinInfo?.Id,
                        Name = cryptoCoin?.CoinInfo?.Name,
                        FullName = cryptoCoin?.CoinInfo?.FullName,
                        MarketCap = cryptoCoin?.RAW?.USD?.MKTCAP,
                    };
                    coins.Add(coin);
                }
            }

            var sortedCoins = coins.OrderByDescending(t => t.MarketCap).ToList();

            return Ok(sortedCoins);
        }
    }
}

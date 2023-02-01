using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinMarketCapController : ControllerBase
    {
        private string? ApiKey { get; }
        private static readonly HttpClient client = new HttpClient();

        public CoinMarketCapController(IConfiguration configuration)
        {
            ApiKey = configuration.GetSection("CoinMarketCap:APIKey").Get<string>();
        }

        [HttpGet("GetCoins")]
        public async Task<ActionResult> GetCoins()
        {
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", ApiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit=5000");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Response failed with status code: " + response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                return NotFound("No response content found");
            }

            var coinMarketCapDTO = JsonConvert.DeserializeObject<CoinMarketCapLatestCoinsDTO>(content);

            return Ok(coinMarketCapDTO);
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult> GetCategories()
        {
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", ApiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/categories?limit=5000");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Response failed with status code: " + response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                return NotFound("No response content found");
            }

            var coinMarketCapDTO = JsonConvert.DeserializeObject<CoinMarketCapCategoriesDTO>(content);

            return Ok(coinMarketCapDTO);
        }
    }
}

using CSharpInWeb3SmartContracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinMarketCapController : ControllerBase
    {
        private string ApiKey { get; }

        public CoinMarketCapController(IConfiguration configuration)
        {
            ApiKey = configuration.GetSection("CoinMarketCap:APIKey").Get<string>();
        }

        [HttpGet("GetCoins")]
        public async Task<ActionResult> GetCoins()
        {
            try
            {
                RestClient restClient = new RestClient("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

                RestRequest restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                restRequest.AddHeader("X-CMC_PRO_API_KEY", ApiKey);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddQueryParameter("limit", "5000");

                RestResponse response = await restClient.ExecuteAsync(restRequest);

                if (response is null)
                {
                    return NotFound("Response is null");
                }

                if (string.IsNullOrEmpty(response.Content))
                {
                    return NotFound("No response content found");
                }

                CoinMarketCapLatestCoinsDTO? coinMarketCapDTO = JsonConvert.DeserializeObject<CoinMarketCapLatestCoinsDTO>(response.Content);

                return Ok(coinMarketCapDTO);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetCategoriesId")]
        public async Task<ActionResult> GetCategoriesId()
        {
            try
            {
                RestClient restClient = new RestClient("https://pro-api.coinmarketcap.com/v1/cryptocurrency/categories");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}

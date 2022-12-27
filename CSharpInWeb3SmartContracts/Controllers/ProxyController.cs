using Microsoft.AspNetCore.Mvc;
using Polly.Fallback;

namespace WebApi.Controllers
{
    // [Route("api/[controller]")]
    [Route("[action]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
        public ProxyController(IHttpClientFactory httpclient)
        {
            _httpClient = httpclient.CreateClient();
        }

        private async Task<ContentResult> ProxyTo(string url)
        {
            return Content(await _httpClient.GetStringAsync(url));
        }

        [HttpGet]
        public async Task<IActionResult> SmartContracts()
        {
            return await ProxyTo("https://localhost:7093/GetAllSmartContracts");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;

namespace WebApi.Controllers
{
    // [Route("api/[controller]")]
    [Route("[action]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
        private readonly AsyncRetryPolicy<IActionResult> _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly AsyncPolicyWrap<IActionResult> _policy;

        public ProxyController(IHttpClientFactory httpclient)
        {
            _httpClient = httpclient.CreateClient();

            _fallbackPolicy = Policy<IActionResult>
                .Handle<Exception>()
                .FallbackAsync(Content("Sorry, we are currently experiencing issues. Please try again later"));

            _retryPolicy = Policy<IActionResult>.Handle<Exception>().RetryAsync();

            _circuitBreakerPolicy ??= Policy.Handle<Exception>().CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

            _policy = Policy<IActionResult>.Handle<Exception>()
                                           .FallbackAsync(Content("Sorry, we are currently experiencing issues. Please try again later"))
                                           .WrapAsync(_retryPolicy)
                                           .WrapAsync(_circuitBreakerPolicy);
        }

        private async Task<IActionResult> ProxyTo(string url)
        {
            return await _fallbackPolicy.ExecuteAsync(async () => Content(await _httpClient.GetStringAsync(url)));
        }

        [HttpGet]
        public async Task<IActionResult> SmartContracts()
        {
            return await ProxyTo("https://localhost:7093/GetAllSmartContracts");
        }


    }
}

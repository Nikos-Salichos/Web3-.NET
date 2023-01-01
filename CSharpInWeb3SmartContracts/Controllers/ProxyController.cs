using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;

namespace WebApi.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
        private readonly AsyncRetryPolicy<IActionResult> _retryPolicy;
        private static AsyncCircuitBreakerPolicy? _circuitBreakerPolicy;
        private readonly AsyncPolicyWrap<IActionResult> _policy;

        public ProxyController(IHttpClientFactory httpclient)
        {
            _httpClient = httpclient.CreateClient("apiGateway");

            _fallbackPolicy = Policy<IActionResult>
                .Handle<Exception>()
                .FallbackAsync(Content("Sorry, we are currently experiencing issues. Please try again later"));

            _retryPolicy = Policy<IActionResult>.Handle<Exception>().RetryAsync(2);

            _circuitBreakerPolicy ??= Policy.Handle<Exception>().CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

            _policy = Policy<IActionResult>.Handle<Exception>()
                               .FallbackAsync(Content("Sorry, we are currently experiencing issues. Please try again later"))
                               .WrapAsync(_retryPolicy)
                               .WrapAsync(_circuitBreakerPolicy);
        }

        private Task<IActionResult> ProxyTo(string url)
        {
            return _retryPolicy.ExecuteAsync(async () => Content(await _httpClient.GetStringAsync(url)));
        }

        [HttpGet]
        public Task<IActionResult> SmartContracts()
        {
            return ProxyTo("https://host.docker.internal:55082/GetAllSmartContracts");
        }


    }
}

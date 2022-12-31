using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.HealthChecks
{
    public class HealthCheck : IHealthCheck
    {
        private const string getAllSmartContractsEndpoint = "https://localhost:7093/GetAllSmartContracts";
        private readonly IHttpClientFactory _httpClientFactory;
        public HealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("CheckHealth")]
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(getAllSmartContractsEndpoint);
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy($"API is running {getAllSmartContractsEndpoint}.");
            }

            return HealthCheckResult.Unhealthy("API is not running");
        }

    }
}

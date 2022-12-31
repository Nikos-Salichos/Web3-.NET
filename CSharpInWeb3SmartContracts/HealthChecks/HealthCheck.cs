using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.HealthChecks
{
    public class HealthCheck : IHealthCheck
    {
        private const string getAllSmartContractsEndpoint = "https://localhost:7093/GetAllSmartContracts";
        private const string getSmartContractById = "https://localhost:7093/GetSmartContractById";

        private readonly IHttpClientFactory _httpClientFactory;
        public HealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(getAllSmartContractsEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Unhealthy($"API is not running {getAllSmartContractsEndpoint}");
            }

            response = await httpClient.GetAsync(getSmartContractById);
            if (!response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Unhealthy($"API is not running {getSmartContractById}");
            }

            return HealthCheckResult.Healthy("All APIs are running.");
        }

    }
}

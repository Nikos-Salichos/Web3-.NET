using AspNetCoreRateLimit;

namespace WebApi.Extensions.Services
{
    public static class RateLimitExtension
    {
        public static void RateLimit(this IApplicationBuilder app)
        {
            app.UseIpRateLimiting();
        }

        public static void AddRateLimiting(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configurationSection);
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
        }
    }
}

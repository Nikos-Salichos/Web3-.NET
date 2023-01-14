using Newtonsoft.Json;
using System.Threading.RateLimiting;

namespace WebApi.Extensions.Services
{
    public static class RateLimitExtension
    {
        public static void RateLimit(this IApplicationBuilder app)
        {
            app.UseRateLimiter();
        }

        public static void AddRateLimiting(this IServiceCollection services, IConfigurationSection configurationSection)
        {

            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,
                            QueueLimit = 0,
                            Window = TimeSpan.FromSeconds(10)
                        }));

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    context.HttpContext.Response.ContentType = "application/json";
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            Error = $"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s)."
                        }), cancellationToken: token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            data = "Too many requests. Please try again later."
                        }), cancellationToken: token);
                    }
                };
            });

        }
    }
}

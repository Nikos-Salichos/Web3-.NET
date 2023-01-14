using Domain.Models;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using System.Threading.RateLimiting;

namespace WebApi.Extensions.Services
{
    public static class RateLimitExtension
    {
        public static RateLimitOptions RateLimitOptions { get; } = new RateLimitOptions();

        public static void RateLimit(this IApplicationBuilder app)
        {
            app.UseRateLimiter();
        }

        public static void AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.GetSection("RateLimitOptions").Bind(RateLimitOptions);

            //Global rate limit
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = RateLimitOptions.AutoReplenishment,
                            PermitLimit = RateLimitOptions.PermitLimit,
                            QueueLimit = RateLimitOptions.QueueLimit,
                            Window = TimeSpan.FromSeconds(RateLimitOptions.Window)
                        }));

                options.RejectionStatusCode = 429;
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

                //Custom rate limiter for endpoint
                options.AddFixedWindowLimiter("ApiSmartContract", options =>
                {
                    options.AutoReplenishment = true;
                    options.PermitLimit = 1;
                    options.Window = TimeSpan.FromSeconds(1);
                });

            });

        }
    }
}

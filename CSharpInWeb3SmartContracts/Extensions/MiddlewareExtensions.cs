using AspNetCoreRateLimit;
using WebApi.CustomMiddleware;

namespace WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static void RateLimit(this IApplicationBuilder app)
        {
            app.UseIpRateLimiting();
        }

        public static void AddRateLimiting(this IServiceCollection services)
        {

        }
    }
}

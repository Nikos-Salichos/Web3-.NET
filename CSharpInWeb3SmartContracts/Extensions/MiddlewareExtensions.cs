using WebApi.CustomMiddleware;

namespace WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            //app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RateLimitingMiddlware>();
        }
    }
}

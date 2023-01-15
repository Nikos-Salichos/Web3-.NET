using WebApi.CustomMiddleware;

namespace WebApi.Extensions.Services
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}

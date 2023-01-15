using Domain.Models;
using System.Net;

namespace WebApi.CustomMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate requestDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.requestDelegate = requestDelegate;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            logger.LogError(exception.ToString());
            var expectionMessageAndInner = exception.Message + " " + exception.InnerException;
            var errorDetail = new ErrorDetail(500, expectionMessageAndInner);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(errorDetail.ToString());
        }
    }
}

using Domain.Enterprise;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Concurrent;
using System.Net;

namespace WebApi.CustomMiddleware
{
    public class RateLimitingMiddlware
    {
        private readonly RequestDelegate _next;
        static readonly ConcurrentDictionary<string, DateTime?> ApiCallsInMemory = new();
        public RateLimitingMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (controllerActionDescriptor is null)
            {
                await _next(context);
                return;
            }

            var apiDecorator = (RateLimitDecorator)controllerActionDescriptor.MethodInfo
                            .GetCustomAttributes(true)
                            .SingleOrDefault(w => w.GetType() == typeof(RateLimitDecorator));

            if (apiDecorator is null)
            {
                await _next(context);
                return;
            }

            string key = GetCurrentClientKey(apiDecorator, context);

            var previousApiCall = GetPreviousApiCallByKey(key);
            if (previousApiCall != null)
            {

                if (DateTime.Now < previousApiCall.Value.AddSeconds(5))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    return;
                }
            }

            UpdateApiCallFor(key);

            await _next(context);
        }

        /// <summary>
        /// We store the time that a client made a call to the current API
        /// </summary>
        /// <param name="key"></param>
        private void UpdateApiCallFor(string key)
        {
            ApiCallsInMemory.TryRemove(key, out _);
            ApiCallsInMemory.TryAdd(key, DateTime.Now);
        }

        private DateTime? GetPreviousApiCallByKey(string key)
        {
            ApiCallsInMemory.TryGetValue(key, out DateTime? value);
            return value;
        }

        /// <summary>
        /// Makes key based on the specified strategy for the current API
        /// </summary>
        /// <param name="apiDecorator"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetCurrentClientKey(RateLimitDecorator apiDecorator, HttpContext context)
        {
            var keys = new List<string>
            {
                context.Request.Path
            };

            if (apiDecorator.RateLimitType == RateLimitType.IpAddress)
            {
                keys.Add(GetClientIpAddress(context));
            }

            // TODO: Implement other strategies.

            return string.Join('_', keys);
        }

        /// <summary>
        /// Returns the client's Ip Address
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetClientIpAddress(HttpContext context)
        {
            // Check this out
            // https://stackoverflow.com/questions/28664686/how-do-i-get-client-ip-address-in-asp-net-core
            return context.Connection.RemoteIpAddress.ToString();
        }
    }
}

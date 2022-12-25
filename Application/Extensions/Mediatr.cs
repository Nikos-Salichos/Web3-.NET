using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.RegisterServices
{
    public static class Mediatr
    {
        public static void RegisterMediatr(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}

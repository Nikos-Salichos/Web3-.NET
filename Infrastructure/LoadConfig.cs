using Autofac;
using Infrastructure.Persistence.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class LoadConfig
    {
        public static void ApplicationStart(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SmartContractRepository>().As<ISmartContractRepository>();
            var container = builder.Build();
        }
    }
}

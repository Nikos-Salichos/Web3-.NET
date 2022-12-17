using Application.Interfaces;
using Application.Services;
using Autofac;

namespace Application
{
    public static class LoadConfig
    {
        public static void ApplicationStart()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SmartContractService>().As<ISmartContractService>();
            var container = builder.Build();
        }
    }
}

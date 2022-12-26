using Application.Interfaces;
using Application.Services;
using Autofac;

namespace Application.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmartContractService>().As<ISmartContractService>().SingleInstance();
            builder.RegisterType<SingletonOptionsService>().As<ISingletonOptionsService>().SingleInstance();
        }
    }
}

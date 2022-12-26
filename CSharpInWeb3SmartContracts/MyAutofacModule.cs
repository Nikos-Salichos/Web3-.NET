using Application.Interfaces;
using Application.Services;
using Autofac;
using Infrastructure.Persistence.Interfaces;
using Infrastructure.Persistence.Repositories;

namespace WebApi
{
    public class MyAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmartContractService>().As<ISmartContractService>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<SingletonOptionsService>().As<ISingletonOptionsService>().SingleInstance();
        }
    }
}

using Autofac;

namespace WebApi
{
    public class MyAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmartContractRepository>().As<ISmartContractRepository>().SingleInstance();
            builder.RegisterType<SmartContractService>().As<ISmartContractService>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<SingletonOptionsService>().As<ISingletonOptionsService>().SingleInstance();
        }
    }
}

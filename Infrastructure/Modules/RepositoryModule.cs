using Autofac;

namespace Infrastructure.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmartContractRepository>().As<ISmartContractRepository>().SingleInstance();
        }
    }
}

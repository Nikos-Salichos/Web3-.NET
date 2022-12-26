using Autofac;
using Infrastructure.Persistence.Interfaces;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<SmartContractRepository>().As<ISmartContractRepository>().SingleInstance();
        }
    }
}

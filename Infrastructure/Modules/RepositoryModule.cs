using Autofac;
using Infrastructure.Persistence.Interfaces;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkRepository>().SingleInstance();
        }
    }
}

using Autofac;
using Autofac.Core;
using System.Reflection;

namespace WebApi.Extensions
{
    public static class RegisterAutoFacModulesExtension
    {
        public static void RegisterAutoFacModules(this ContainerBuilder builder)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            string exeAssemblyName = Assembly.GetEntryAssembly().GetName().Name;

            Directory
            .GetFiles(path, exeAssemblyName, SearchOption.TopDirectoryOnly)
            .Select(Assembly.LoadFrom)
            .ToList()
            .ForEach(assembly =>
            {
                assembly.GetTypes()
                        .Where(p => typeof(IModule).IsAssignableFrom(p) && !p.IsAbstract)
                        .Select(p => (IModule)Activator.CreateInstance(p))
                        .ToList()
                        .ForEach(module => builder.RegisterModule(module));
            });
        }
    }
}

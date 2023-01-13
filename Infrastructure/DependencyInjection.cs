using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddDbContext<MsqlDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MsSqlConnection"),
                b => b.MigrationsAssembly(typeof(MsqlDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<IMsqlSqlDbContext>(provider => provider.GetService<MsqlDbContext>());
            return services;
        }
    }
}

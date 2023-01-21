using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DatabaseServiceRegistration
    {
        public static IServiceCollection RegisterDatabases(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddDbContext<MsqlDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MsSqlConnection"),
                b => b.MigrationsAssembly(typeof(MsqlDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<IMsSqlDbContext>(provider => provider.GetService<MsqlDbContext>());

            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"),
                b => b.MigrationsAssembly(typeof(PostgreSqlDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<IPostgreSqlDbContext>(provider => provider.GetService<PostgreSqlDbContext>());

            services.AddDbContext<SqliteDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SqliteConnection"),
            b => b.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<ISqliteDbContext>(provider => provider.GetService<SqliteDbContext>());

            return services;
        }
    }
}

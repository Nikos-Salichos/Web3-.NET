﻿using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class DatabaseServiceRegistration
    {
        public static IServiceCollection RegisterDatabases(this IServiceCollection services,
           IConfiguration configuration)
        {
            //MsSQL
            services.AddDbContext<MsSqlDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MsSqlConnection"),
                b => b.MigrationsAssembly(typeof(MsSqlDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<IMsSqlDbContext>(provider => provider.GetService<MsSqlDbContext>() ?? throw new ArgumentNullException(nameof(services)));

            //PostgreSql
            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreSqlDbContext>(options =>
            services.AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"),
                b => b.MigrationsAssembly(typeof(PostgreSqlDbContext).Assembly.FullName)), ServiceLifetime.Transient));

            services.AddScoped<IPostgreSqlDbContext>(provider => provider.GetService<PostgreSqlDbContext>() ?? throw new ArgumentNullException(nameof(services)));

            //Sqlite
            services.AddDbContext<SqliteDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SqliteConnection"),
                b => b.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<ISqliteDbContext>(provider => provider.GetService<SqliteDbContext>() ?? throw new ArgumentNullException(nameof(services)));

            //Redis cache
            services.AddStackExchangeRedisCache(options =>
              {
                  options.InstanceName = "SmartContracts";
                  options.ConfigurationOptions = new ConfigurationOptions
                  {
                      EndPoints = { configuration["ConnectionStrings:Redis"] },
                      AbortOnConnectFail = false,
                      ConnectTimeout = 5000,
                      ConnectRetry = 5,
                  };
              });

            //  services.AddSingleton(sp => ConnectionMultiplexer.Connect(configuration["ConnectionStrings:Redis"]));

            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());

            return services;
        }
    }
}

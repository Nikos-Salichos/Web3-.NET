using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace WebApi.Logging
{
    public static class SerilogRegistration
    {
        public static void SerilogConfiguration(WebApplicationBuilder builder)
        {
            var serilogConfiguration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                    .Build();

            var path = serilogConfiguration.GetSection("Serilog").GetSection("WriteTo").GetSection("1").GetSection("Args").GetSection("path").Value;
            var finalPath = Path.Combine(Environment.CurrentDirectory + path);

            var minimumLevel = serilogConfiguration.GetSection("Serilog").GetSection("MinimumLevel").Value;
            var rollingInterval = (RollingInterval)Enum.Parse(typeof(RollingInterval), serilogConfiguration!.GetSection("Serilog").GetSection("RollingInterval").Value!);
            var levelSwitch = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minimumLevel!);

            LoggingLevelSwitch loggingLevelSwitch = new();
            loggingLevelSwitch.MinimumLevel = levelSwitch;

            builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(serilogConfiguration)
                                                     .MinimumLevel.ControlledBy(loggingLevelSwitch)
                                                     .WriteTo.File(new JsonFormatter(), finalPath,
                                                     rollingInterval: rollingInterval));

            /*.WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection"),
                new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    SchemaName = "dbo",
                    AutoCreateSqlTable = true
                }));*/

        }
    }
}

using Serilog;
using Serilog.Formatting.Json;
using System.Data.SQLite;

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

            var rollingInterval = (RollingInterval)Enum.Parse(typeof(RollingInterval), serilogConfiguration!.GetSection("Serilog").GetSection("RollingInterval").Value!);

            var serilogSqliteDbName = serilogConfiguration.GetSection("Serilog").GetSection("WriteTo").GetSection("2").GetSection("Args").GetSection("connectionString").Value;
            if (serilogSqliteDbName != null)
            {
                string value = serilogSqliteDbName.Substring(serilogSqliteDbName.IndexOf('=') + 1);
                var serilogSqlitePath = Environment.CurrentDirectory + "\\Logs\\" + value;
                if (!File.Exists(serilogSqlitePath))
                {
                    SQLiteConnection.CreateFile(serilogSqlitePath);
                }
                var sqliteSerilogTable = serilogConfiguration.GetSection("Serilog").GetSection("WriteTo").GetSection("2").GetSection("Args").GetSection("tableName").Value;
                builder.Host.UseSerilog((ctx, lc) => lc
                            .ReadFrom.Configuration(serilogConfiguration)
                            .WriteTo.File(new JsonFormatter(), finalPath,
                                          rollingInterval: rollingInterval)
                            .WriteTo.SQLite(serilogSqlitePath, sqliteSerilogTable));
            }
        }
    }
}

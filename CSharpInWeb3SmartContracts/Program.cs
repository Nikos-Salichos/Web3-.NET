using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Text.Json.Serialization;
using WebApi.GraphQL;
using WebApi.Utilities;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

//Extension method for configuring CORS
IServiceCollection configureCors = builder.Services.ConfigureCors();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())).AddNewtonsoftJson();

builder.Services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient("https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v3", new NewtonsoftJsonSerializer()));
builder.Services.AddScoped<UniswapV3GraphQL>();

// Read appsettings.json file
IConfigurationRoot? configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

#region Serilog Logging
string fullPath = Environment.CurrentDirectory + @"\logs.txt";
LoggingLevelSwitch? levelSwitch = new();
levelSwitch.MinimumLevel = LogEventLevel.Debug;
builder.Host.UseSerilog((ctx, lc) => lc.MinimumLevel.ControlledBy(levelSwitch)
                                       .WriteTo.Console()
                                       .WriteTo.File(fullPath, rollingInterval: RollingInterval.Day));
/*.WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection"),
 new MSSqlServerSinkOptions
 {
     TableName = "Logs",
     SchemaName = "dbo",
     AutoCreateSqlTable = true
 }));*/
#endregion Serilog Logging

//Load Controllers dynamically from DLL
/*Assembly? assembly = Assembly.LoadFile(@"C:\Users\Nikos\source\repos\LoadDynamicControllers\LoadDynamicControllers\bin\Debug\net6.0\Test.dll");
if (assembly != null)
{
    builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
    builder.Services.AddMvc().AddApplicationPart(assembly).AddControllersAsServices();
}*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.UseInlineDefinitionsForEnums();
    options.SchemaFilter<EnumSchemaFilter>();
});

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(logger);

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

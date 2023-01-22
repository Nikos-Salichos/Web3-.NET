using Application;
using Application.Extensions;
using Application.Mappers;
using Application.Modules;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Models;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Infrastructure;
using Infrastructure.Modules;
using Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Text.Json.Serialization;
using WebApi.Extensions.Services;
using WebApi.GraphQL;
using WebApi.HealthChecks;
using WebApi.Logging;
using WebApi.Utilities;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

#region Cors
IServiceCollection configureCors = builder.Services.ConfigureCors();
#endregion Cors

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())).AddNewtonsoftJson();

builder.Services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient("https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v3", new NewtonsoftJsonSerializer()));
builder.Services.AddScoped<UniswapV3GraphQL>();

#region AppSettings.json
IConfigurationRoot? configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
#endregion AppSettings.json

#region Serilog Logging
SerilogRegistration.SerilogConfiguration(builder);
#endregion Serilog Logging

#region Database
builder.Services.RegisterDatabases(builder.Configuration);
#endregion Database

#region Autofac Dependency Injection
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new ServiceModule()));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new RepositoryModule()));
#endregion Autofac Dependency Injection

#region AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
#endregion AutoMapper

#region Response Cache Profiles
builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("DefaultCache", new CacheProfile() { Duration = 5 });
});
#endregion Response Cache Profiles

#region Mediatr
builder.Services.RegisterMediatr();
#endregion Mediatr

#region FluentValidation
builder.Services.AddFluentValidation();
#endregion FluentValidation

#region Api Gateway Pattern (Proxy Controller)
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("apiGateway").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
});
#endregion Api Gateway Pattern (Proxy Controller)

#region Rate Limit
builder.Services.AddRateLimiting(builder.Configuration);
#endregion Rate Limit

#region Read User appsettings.json
builder.Services.AddOptions<WalletOwner>().BindConfiguration("User").ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddOptions<NetworkProvider>().BindConfiguration("NetworkProvider").ValidateDataAnnotations().ValidateOnStart();
#endregion Read User appsettings.json

#region Health Checks
builder.Services.AddHealthChecks().AddDbContextCheck<MsqlDbContext>();
builder.Services.AddHealthChecks().AddCheck<HealthCheck>("Custom Health Checks");
#endregion Health Checks

#region Database Exception Filter
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endregion Database Exception Filter

#region Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

#endregion Response Compression

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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.RateLimit();

app.UseGlobalExceptionMiddleware();

app.UseCors();

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

app.Run();

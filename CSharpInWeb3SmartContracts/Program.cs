using CSharpInWeb3SmartContracts;
using CSharpInWeb3SmartContracts.Utilities;
using System.Text.Json.Serialization;



WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

//Extension method for configuring CORS
IServiceCollection configureCors = builder.Services.ConfigureCors();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


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


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x => x.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();

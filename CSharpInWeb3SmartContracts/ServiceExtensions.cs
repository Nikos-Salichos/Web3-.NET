namespace CSharpInWeb3SmartContracts
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureCORS(this IServiceCollection services)
            => services.AddCors(options => options.AddPolicy("AllowOrigins", builder => builder
                .WithOrigins("http://localhost:7093")
                .AllowAnyMethod()
                .AllowAnyHeader()));
    }
}

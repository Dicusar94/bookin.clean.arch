using Swashbuckle.AspNetCore.Filters;

namespace BookingApp.Infrastructure.SwaggerConfigs;

public static class SwaggerConfigurationBuilderExtensions
{
    
    public static IHostApplicationBuilder ConfigureSwagger(this IHostApplicationBuilder builder)
    {
        // ✅ Register the configuration class
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

        // ✅ Add Swashbuckle (configuration happens via ConfigureSwaggerOptions)
        builder.Services.AddSwaggerGen();

        // ✅ Register example providers
        builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
        
        return builder;
    }
}
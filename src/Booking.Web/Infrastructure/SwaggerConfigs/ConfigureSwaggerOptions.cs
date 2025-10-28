using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookingApp.Infrastructure.SwaggerConfigs;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        // ✅ Auto-generate a document for each discovered API version
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }

        // ✅ Filter endpoints by version (SIMPLIFIED!)
        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            // Use the GroupName from API explorer (already set by .AddApiExplorer)
            return apiDesc.GroupName == docName;
        });

        // ✅ Enable example filters
        options.ExampleFilters();
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Booking API",
            Version = description.ApiVersion.ToString(),
            Description = $"Booking application API - Version {description.ApiVersion}",
            Contact = new OpenApiContact
            {
                Name = "Your Team",
                Email = "support@booking.com"
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " (Deprecated)";
        }

        return info;
    }
}
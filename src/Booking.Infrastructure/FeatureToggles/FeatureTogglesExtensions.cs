using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

namespace Booking.Infrastructure.FeatureToggles;

public static class FeatureTogglesExtensions
{
    public static IHostApplicationBuilder AddFeatureToggles(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScopedFeatureManagement();

        builder.Services.Configure<ConfigurationFeatureDefinitionProviderOptions>(opt =>
        {
            // merge custom configurations
            opt.CustomConfigurationMergingEnabled = true;
        });

        return builder;
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Configuration.ConfigServer;

namespace Booking.Infrastructure.ExternalConfigs;

public static class ExternalConfigsBuilderExtensions
{
    public static IHostApplicationBuilder AddExternalConfigurations(this IHostApplicationBuilder builder)
    {
        var options = builder.Configuration
            .GetSection(ExternalConfigOptions.SectionName)
            .Get<ExternalConfigOptions>()!;

        if (options.Enabled)
        {
            // note: In case running in testing mode
            return builder;
        }

        builder.AddConfigServer();
        builder.Services.Configure<AppInfoOptions>(
            builder.Configuration.GetSection(AppInfoOptions.SectionName));

        return builder;
    }
}
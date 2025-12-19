using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BookingApp.Telemetry;

public static class TelemetryAppBuilderExtensions
{
    public static IHostApplicationBuilder AddTelemetry(this IHostApplicationBuilder builder)
    {
        // Add telemetry configuration here in the future
        OpenTelemetrySettings settings = builder
            .Configuration
            .GetSection(nameof(OpenTelemetrySettings))
            .Get<OpenTelemetrySettings>()!;

        if(!settings.Enabled)
        {
            return builder;
        }
        builder.Logging.AddOpenTelemetry(config =>
        {
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(
                    serviceName: RunTimeDiagnosticConfig.ServiceName,
                    serviceVersion: RunTimeDiagnosticConfig.ServiceVersion);
            config.SetResourceBuilder(resourceBuilder);
            config.IncludeScopes = true;
            config.IncludeFormattedMessage = true;
            config.ParseStateValues = true;
            config.AddOtlpExporter(exporterOptions =>
            {
                exporterOptions.Endpoint = settings.TracesEndpoint;
            });
        });


        builder.Services.AddOpenTelemetry()
            .ConfigureResource(res => res.AddService(
                serviceName: RunTimeDiagnosticConfig.ServiceName,
                serviceVersion: RunTimeDiagnosticConfig.ServiceVersion
            ))
            .WithTracing(tracingProviderBuilder => 
                tracingProviderBuilder
                    .AddSource(RunTimeDiagnosticConfig.Source.Name)
                    .AddNpgsql()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = settings.TracesEndpoint;
                        options.Protocol = OtlpExportProtocol.Grpc;
                    })
                    .AddConsoleExporter()
            )
            .WithMetrics(meterProviderBuilder =>
                meterProviderBuilder
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = settings.TracesEndpoint;
                        options.Protocol = OtlpExportProtocol.Grpc;
                    })
                    .AddMeter(
                        "System.Runtime",
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        RunTimeDiagnosticConfig.Meter.Name
                    )
        );

        builder.Services.AddSingleton(RunTimeDiagnosticConfig.Propagator);

        return builder;
    }
}
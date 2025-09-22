using Booking.Infrastructure;

namespace Booking.Web;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWeb(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        
        return builder.AddInfrastructure();
    }
}
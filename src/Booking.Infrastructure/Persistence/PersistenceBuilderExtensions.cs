using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookingApp.Persistence;

public static class PersistenceBuilderExtensions
{
    internal static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(opt => 
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

        return builder;
    }
}
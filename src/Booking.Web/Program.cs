using BookingApp;
using BookingApp.Features.Bookings.Backgrounds;
using BookingApp.Infrastructure.Endpoints;
using BookingApp.Persistence;
using TickerQ.DependencyInjection;
using TickerQ.Utilities.Interfaces.Managers;

var builder = WebApplication.CreateBuilder(args);
{
    builder.AddPresentation();
}

var app = builder.Build();
{
    
    app.UseHttpsRedirection();
    
    if (app.Environment.IsDevelopment())
    {
        app.ApplyMigrations();
    }
    
    app.UseMiddlewares();
    
    app.UseSwagger(); // This creates /swagger/v1/swagger.json
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API v1");
        options.RoutePrefix = "swagger"; // Access at /swagger
    });
    
    app.UseTickerQ();
    app.MapEndpointsFrom<Program>();
    app.Run();
}
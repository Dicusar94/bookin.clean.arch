using BookingApp;
using BookingApp.Infrastructure.Endpoints;
using TickerQ.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.AddPresentation();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
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
using Booking.Web;

var builder = WebApplication.CreateBuilder(args);
{
    builder.AddWeb();
}

var app = builder.Build();
{

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.MapGet("/weatherforecast", () => new { test = "test" })
        .WithName("GetWeatherForecast");

    app.Run();
}
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
    app.UseMiddlewares();
    
    app.Run();
}
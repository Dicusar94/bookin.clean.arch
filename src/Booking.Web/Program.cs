using Booking.Application;
using Booking.Core.Messaging;
using Booking.Infrastructure.ExternalConfigs;
using Booking.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

    app.MapGet("/send-message/{name}", ([FromRoute] string name, IMessageProducer messageProducer) =>
        {
            var testEvent = new TestEvent
            {
                Name = name
            };

            var header = new Header(
                sourceCode: "booking_web",
                eventCode: testEvent.GetType().Name);

            var message = new Message(header, testEvent);
            
            messageProducer.PublishMessage(message, testEvent.RoutingKey);

            return new { Name = name };
        })
        .WithName("send-message");
    
    app.MapGet("/get-config", (IOptionsSnapshot<AppInfoOptions> options) => options.Value)
        .WithName("get-config");

    app.Run();
}
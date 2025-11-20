using BookingApp.Entities;
using BookingApp.Messaging;
using BookingApp.Telemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookingApp.Persistence.Interceptors;

public class PublishDomainEventsInterceptor(IMessageProducer messageProducer) : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, 
        int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is not null)
        {
            PublishDomainEvents(eventData.Context);
        }
        
        return ValueTask.FromResult(result);
    }

    private void PublishDomainEvents(DbContext context)
    {
        using var activity = RunTimeDiagnosticConfig.Source.StartActivity("Checking for events to publish");

        var domainEvents = context
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entity => entity.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents.ToList();
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        activity?.SetTag("countOfEventsToPublish", domainEvents.Count);

        foreach (var domainEvent in domainEvents)
        {
            using var subActivity = RunTimeDiagnosticConfig.Source
                .StartActivity($"Publishing {domainEvent.GetType().Name}");

            subActivity?.SetTag("RoutingKey", domainEvent.RoutingKey);

            var header = new Header(
                sourceCode: "booking_web",
                eventCode: domainEvents.GetType().Name);

            var message = new Message(header: header, body: domainEvent);
            
            messageProducer.PublishMessage(message, domainEvent.RoutingKey);
            
            subActivity?.Stop();
        }
    }
}
using BookingApp.Abstractions;
using BookingApp.BookingAggregate.Events;
using BookingApp.Features.Bookings.Backgrounds;
using BookingApp.Messaging;
using Microsoft.Extensions.DependencyInjection;
using TickerQ.Utilities;
using TickerQ.Utilities.Interfaces.Managers;

namespace BookingApp.Features.Bookings.Events.Handlers;

public class ScheduleBookingAutoCancelEventHandler(IServiceScopeFactory serviceScopeFactory) 
    : BaseListener<BookingPendingConfirmationEvent>(serviceScopeFactory)
{
    public override async Task ProcessMessage(Message message, string routingKey)
    {
        var timeTickerManager = serviceProvider.GetRequiredService<ITimeTickerManager<BookingAutoCancelTicker>>();

        var evt = message.GetBody<BookingPendingConfirmationEvent>();
        var executionTime = evt.OnDate.DateTime.Add(BookingAggregate.Booking.MaxPendingStatusDuration);
        
        var ticker = new BookingAutoCancelTicker
        {
            BookingId = evt.Id,
            Function = nameof(BookingAutoCancelTicker),
            ExecutionTime = executionTime,
            Request = TickerHelper.CreateTickerRequest(evt.Id),
            Retries = 0,
            RetryIntervals = []
        };

        await timeTickerManager.AddAsync(ticker);
    }
}
using BookingApp.Shared;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Models;

namespace BookingApp.Features.Bookings.BackgroundJobs.AutoCancels;

public class BookingAutoCancelFunctions(IUnitOfWork unitOfWork, TimeProvider timeProvider)
{
    [TickerFunction(functionName: nameof(BookingAutoCancelTicker))]
    public async Task Execute(TickerFunctionContext<Guid> context, CancellationToken ct)
    {
        var bookingId = context.Request;
        var booking = await unitOfWork.Bookings.GetBookingById(bookingId);
        booking.AutoCancel(timeProvider);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
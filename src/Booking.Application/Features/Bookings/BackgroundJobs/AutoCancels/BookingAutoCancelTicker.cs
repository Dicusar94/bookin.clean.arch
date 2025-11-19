
using TickerQ.Utilities.Models.Ticker;

namespace BookingApp.Features.Bookings.BackgroundJobs.AutoCancels;

public class BookingAutoCancelTicker : TimeTicker
{
    public Guid BookingId { get; set; }
}
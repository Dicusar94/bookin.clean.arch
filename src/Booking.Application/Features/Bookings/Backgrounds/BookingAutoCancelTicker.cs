using TickerQ.Utilities.Models.Ticker;

namespace BookingApp.Features.Bookings.Backgrounds;

public class BookingAutoCancelTicker : TimeTicker
{
    public Guid BookingId { get; set; }
}
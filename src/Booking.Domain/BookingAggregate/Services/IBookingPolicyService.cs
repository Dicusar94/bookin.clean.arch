namespace BookingApp.BookingAggregate.Services;

public interface IBookingPolicyService
{
    Task EnsureBookingCanBeCreatedAsync(Booking booking, CancellationToken ct = default);
}
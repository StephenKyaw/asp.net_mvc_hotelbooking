using Domain.Entities.Bookings;

namespace Domain.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingList();
        Task<Booking> GetBookingById(string id);
        Task AddBooking(Booking booking);
    }
}

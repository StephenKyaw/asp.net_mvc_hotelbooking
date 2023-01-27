using Domain.Entities.Bookings;
using Domain.Interfaces;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<BookingItem> _bookingItemRepository;

        public BookingService(IRepository<Booking> bookingRepository, IRepository<BookingItem> bookingItemRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingItemRepository = bookingItemRepository;
        }

        public async Task AddBooking(Booking booking)
        {
            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingById(string id)
        {
            return await _bookingRepository.FirstOrDefaultAsync(x => x.BookingID == id, "BookingItems");
        }

        public async Task<IEnumerable<Booking>> GetBookingList()
        {
            return await _bookingRepository.GetAllAsync("BookingItems");
        }
    }
}

using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IRepository<Hotel> _hotelRepository;
        private readonly IRepository<HotelPhoto> _hotelPhotoRepository;

        public HotelService(IRepository<Hotel> hotelRepository, IRepository<HotelPhoto> hotelPhotoRepository)
        {
            _hotelRepository = hotelRepository;
            _hotelPhotoRepository = hotelPhotoRepository;
        }

        public async Task AddHotel(Hotel hotel)
        {
            await _hotelRepository.AddAsync(hotel);
            await _hotelRepository.SaveChangesAsync();
        }

        public async Task DeleteHotel(string id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);

            _hotelRepository.Remove(hotel);

            await _hotelRepository.SaveChangesAsync();
        }

        public async Task DeleteHotelPhotosByHotelId(string hoteId)
        {
            var photos = await _hotelPhotoRepository.GetAsync(x => x.HotelId == hoteId);

            if (photos != null)
            {
                _hotelPhotoRepository.RemoveRange(photos);
                await _hotelPhotoRepository.SaveChangesAsync();
            }
        }

        public async Task<Hotel> GetHotelById(string id)
        {
            return await _hotelRepository.FirstOrDefaultAsync(x => x.HotelId == id, "City,Township,HotelPhotos");
        }

        public async Task<IEnumerable<HotelPhoto>> GetHotelPhotosByHotelId(string hotelId)
        {
            return await _hotelPhotoRepository.GetAsync(x => x.HotelId == hotelId);
        }

        public async Task<IEnumerable<Hotel>> GetHotels()
        {
            return await _hotelRepository.GetAllAsync("City,Township,HotelPhotos");
        }

        public async Task UpdateHotel(Hotel hotel)
        {
            _hotelRepository.Update(hotel);
            await _hotelRepository.SaveChangesAsync();
        }
    }
}

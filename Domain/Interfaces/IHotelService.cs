using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetHotels();
        Task<Hotel> GetHotelById(string id);
        Task<IEnumerable<HotelPhoto>> GetHotelPhotosByHotelId(string hotelId);
        Task AddHotel(Hotel hotel);
        Task UpdateHotel(Hotel hotel);
        Task DeleteHotel(string id);
        Task DeleteHotelPhotosByHotelId(string hoteId);
        
    }
}

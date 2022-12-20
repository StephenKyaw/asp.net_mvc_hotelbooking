using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILocationService
    {
        Task AddLocation(LocationDto location);
        Task<IEnumerable<LocationDto>> GetLocations();

        Task<IEnumerable<City>> GetCities();
        Task<IEnumerable<Township>> GetTownships();
        Task<IEnumerable<Township>> GetTownshipsByCityName(string cityName);
    }
}

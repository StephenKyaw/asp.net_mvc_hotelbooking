using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<City>> GetCities();
        Task<City> GetCityById(string id);
        Task AddCity(City city);
        Task UpdateCity(City city);
        Task DeleteCity(string cityId);

        Task<IEnumerable<Township>> GetTownships();
        Task<Township> GetTownshipById(string id);
        Task AddTownship(Township township);
        Task UpdateTownship(Township township);
        Task DeleteTownship(string townshipId);
    }
}

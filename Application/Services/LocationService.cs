using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Township> _townshipRepository;

        public LocationService(IRepository<City> cityRepository, IRepository<Township> townshipRepository)
        {
            _cityRepository = cityRepository;
            _townshipRepository = townshipRepository;
        }

        public async Task AddCity(City city)
        {
            await _cityRepository.AddAsync(city);
            await _cityRepository.SaveChangesAsync();
        }

        public async Task AddTownship(Township township)
        {
            await _townshipRepository.AddAsync(township);
            await _townshipRepository.SaveChangesAsync();
        }

        public async Task DeleteCity(string cityId)
        {
            var city = await _cityRepository.GetByIdAsync(cityId);
            _cityRepository.Remove(city);
            await _cityRepository.SaveChangesAsync();
        }

        public async Task DeleteTownship(string townshipId)
        {
            var township = await _townshipRepository.GetByIdAsync(townshipId);
            _townshipRepository.Remove(township);
            await _townshipRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _cityRepository.GetAllAsync("Townships");
        }

        public async Task<City> GetCityById(string id)
        {
            return await _cityRepository.GetByIdAsync(id);
        }

        public async Task<Township> GetTownshipById(string id)
        {
            return await _townshipRepository.FirstOrDefaultAsync(x => x.TownshipId == id, "City");
        }

        public async Task<IEnumerable<Township>> GetTownships()
        {
            return await _townshipRepository.GetAllAsync("City");
        }

        public async Task UpdateCity(City city)
        {
            _cityRepository.Update(city);
            await _cityRepository.SaveChangesAsync();
        }

        public async Task UpdateTownship(Township township)
        {
            _townshipRepository.Update(township);
            await _townshipRepository.SaveChangesAsync();
        }
    }
}

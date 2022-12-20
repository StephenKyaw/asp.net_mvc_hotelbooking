using Domain.Dtos;
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

        public async Task AddLocation(LocationDto location)
        {
            var city = new City { CityId = Guid.NewGuid().ToString(), CityName = location.CityName, Created = DateTime.Now, CreatedBy = location.CreatedBy };

            city.Townships = location.Townships.Select(x => new Township { TownshipId = Guid.NewGuid().ToString(), TownshipName = x, CityId = city.CityId, Created = DateTime.Now, CreatedBy = location.CreatedBy }).ToList();

            await _cityRepository.AddAsync(city);

            await _cityRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<IEnumerable<LocationDto>> GetLocations()
        {
            List<LocationDto> locations = new List<LocationDto>();

            var cities = await _cityRepository.GetAllAsync("Townships");

            foreach (var city in cities)
            {
                locations.Add(new LocationDto(city.CityName, city.CreatedBy,
                    city.Townships.Select(x => x.TownshipName).ToList()));
            }

            return locations;
        }

        public async Task<IEnumerable<Township>> GetTownships()
        {
            return await _townshipRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Township>> GetTownshipsByCityName(string cityName)
        {
            var city = await _cityRepository.FirstOrDefaultAsync(x => x.CityName == cityName);

            return await _townshipRepository.GetAsync(x => x.CityId == city.CityId);
        }
    }
}

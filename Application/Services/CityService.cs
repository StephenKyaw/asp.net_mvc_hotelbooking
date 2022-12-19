using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class CityService : ICityService
    {
        private readonly IRepository<City> _cityRepository;

        public CityService(IRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<CityDto>> GetAll()
        {
            var query = await _cityRepository.GetAllAsync();

            return query.Select(x => new CityDto { Id = x.CityId, Name = x.CityName }).ToList();
        }

        public async Task Insert(CityDto dto)
        {
            var city = new City()
            {
                CityId = Guid.NewGuid().ToString(),
                CityName = dto.Name
            };

            await _cityRepository.AddAsync(city);
            await _cityRepository.SaveChangesAsync();
        }
    }
}


using Domain.Dtos;
namespace Domain.Interfaces
{
    public interface ICityService
    {
        Task Insert(CityDto city );
        Task<IEnumerable<CityDto>> GetAll();
    }
}

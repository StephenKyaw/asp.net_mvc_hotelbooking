using Domain.Common;

namespace Domain.Entities
{
    public class City : BaseAuditEntity
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
        public IEnumerable<Township> Townships { get; set; } = new List<Township>();

    }
}

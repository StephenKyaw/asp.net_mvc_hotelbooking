using Domain.Common;

namespace Domain.Entities
{
    public class Township : BaseAuditEntity
    {
        public string TownshipId { get; set; }
        public string TownshipName { get; set; }
        public string CityId { get; set; }
        public City City { get; set; }
    }
}

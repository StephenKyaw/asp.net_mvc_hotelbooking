using Domain.Common;

namespace Domain.Entities
{
    public class Township : BaseAuditEntity
    {
        public string TownshipId { get; set; }
        public string TownshipName { get; set; }
        public string CityId { get; set; }
        public City City { get; set; }

        public virtual IEnumerable<Hotel> Hotels { get; set; }

        public Township()
        {
            TownshipId= Guid.NewGuid().ToString();
            CreatedDate= DateTime.Now;
        }
    }
}

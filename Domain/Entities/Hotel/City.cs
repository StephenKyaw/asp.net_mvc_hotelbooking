using Domain.Common;

namespace Domain.Entities
{
    public class City : BaseAuditEntity
    {
        public string CityId { get; set; }
        public string CityName { get; set; }

        public virtual IEnumerable<Township> Townships { get; set; }
        public virtual IEnumerable<Hotel> Hotels { get; set; }

        public City()
        {
            CityId = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }
    }
}

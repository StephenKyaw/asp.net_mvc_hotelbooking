using Domain.Common;

namespace Domain.Entities
{
    public class FacilityType : BaseAuditEntity
    {
        public string FacilityTypeId { get; set; }
        public string FacilityTypeName { get; set; }

        public virtual RoomFacility RoomFacility { get;set;}
        public FacilityType()
        {
            FacilityTypeId = Guid.NewGuid().ToString();
            CreatedDate= DateTime.Now;
        }
    }
}

using Domain.Common;

namespace Domain.Entities
{
    public class RoomFacility : BaseEntity
    {
        public string RoomFacilityId { get; set; }        
        public string FacilityTypeId { get; set; }
        public FacilityType FacilityType { get; set; }
        public string RoomId { get; set; }
        public Room Room { get; set; }

        public RoomFacility()
        {
            RoomFacilityId= Guid.NewGuid().ToString();
        }
    }
}

using Domain.Common;

namespace Domain.Entities
{
    public class RoomFacility : BaseEntity
    {
        public string? RoomFacilityId { get; set; }
        public string? RoomId { get; set; }
        public string? FacilityTypeId { get; set; }
    }
}

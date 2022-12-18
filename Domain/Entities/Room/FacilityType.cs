using Domain.Common;

namespace Domain.Entities
{
    public class FacilityType : BaseAuditEntity
    {
        public string? FacilityTypeId { get; set; }

        public string? FacilityTypeName { get; set; }
    }
}

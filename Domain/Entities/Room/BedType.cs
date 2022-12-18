using Domain.Common;

namespace Domain.Entities
{
    public class BedType : BaseAuditEntity
    {
        public string? BedTypeId { get; set; }
        public string? BedTypeName { get; set; }
    }
}

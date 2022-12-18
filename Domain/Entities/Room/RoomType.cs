using Domain.Common;

namespace Domain.Entities
{
    public class RoomType : BaseAuditEntity
    {
        public string? RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; }
    }
}

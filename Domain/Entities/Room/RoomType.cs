using Domain.Common;

namespace Domain.Entities
{
    public class RoomType : BaseAuditEntity
    {
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }

        public virtual IEnumerable<Room> Rooms { get; set; }

        public RoomType()
        {
            RoomTypeId = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }
    }
}

using Domain.Common;

namespace Domain.Entities
{
    public class RoomPhoto : BaseAuditEntity
    {
        public string RoomPhotoId { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public string RoomId { get; set; }
        public Room Room { get; set; }

        public RoomPhoto()
        {
            RoomPhotoId= Guid.NewGuid().ToString();
            CreatedDate= DateTime.Now;
        }
    }
}

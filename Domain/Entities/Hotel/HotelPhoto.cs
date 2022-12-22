using Domain.Common;

namespace Domain.Entities
{
    public class HotelPhoto : BaseAuditEntity
    {
        public string HotelPhotoId { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public string HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public HotelPhoto()
        {
            HotelPhotoId = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }
    }
}

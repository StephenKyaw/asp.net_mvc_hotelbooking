using Domain.Common;

namespace Domain.Entities
{
    public class BedType : BaseAuditEntity
    {
        public string BedTypeId { get; set; }
        public string BedTypeName { get; set; }
        public virtual RoomBed RoomBed { get; set; }

        public BedType()
        {
            BedTypeId= Guid.NewGuid().ToString();
            CreatedDate= DateTime.Now;
        }
    }
}

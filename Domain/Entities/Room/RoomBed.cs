using Domain.Common;

namespace Domain.Entities
{
    public class RoomBed : BaseEntity
    {
        public string RoomBedId { get; set; }       
        public string BedTypeId { get; set; }
        public BedType BedType { get; set; }


        public int NumberOfBeds { get; set; }

        public string RoomId { get; set; }
        public Room Room { get;set;}

        public RoomBed()
        {
            RoomBedId= Guid.NewGuid().ToString();            
        }
    }
}

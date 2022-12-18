using Domain.Common;

namespace Domain.Entities
{
    public class Room : BaseAuditEntity
    {
        public string? RoomId { get; set; }
        public string? HotelId { get; set; }
        public string? RoomTypeId { get; set; }
        public decimal Price { get; set; }
        public double Rate { get; set; }
        public int NumberOfRooms { get; set; }
        public IEnumerable<RoomBed> RoomBeds { get; set; } = new List<RoomBed>();
        public IEnumerable<RoomPhoto> RoomPhotos { get; set; } = new List<RoomPhoto>();
        public IEnumerable<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panda.HotelBooking.Models
{
    public class RoomFacility
    {
        [Key]
        public string RoomFacilityId { get; set; }

        public string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public string FacilityTypeId { get; set; }

        [ForeignKey("FacilityTypeId")]
        public FacilityType FacilityType { get; set; }
    }
}

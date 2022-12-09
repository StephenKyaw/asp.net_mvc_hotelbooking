using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panda.HotelBooking.Models
{
    public class RoomPhoto
    {
        [Key]
        public string RoomPhotoId { get; set; }

        public string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public string PhotoPath { get; set; }
    }
}

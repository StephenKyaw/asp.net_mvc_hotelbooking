using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panda.HotelBooking.Models
{
    public class RoomBed
    {
        [Key]
        public string RoomBedId { get; set; }

        public string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        [Display(Name = "Bed Type")]
        [Required(ErrorMessage = "Require Bed Type.")]
        public string BedTypeId { get; set; }

        [ForeignKey("BedTypeId")]
        public BedType BedType { get; set; }

        [Display(Name = "Number of Beds")]
        [Required(ErrorMessage = "Require Number of Beds.")]
        public int NumberOfBeds { get; set; }
    }

}

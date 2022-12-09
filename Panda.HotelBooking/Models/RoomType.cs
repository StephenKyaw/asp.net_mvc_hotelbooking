using System.ComponentModel.DataAnnotations;

namespace Panda.HotelBooking.Models
{
    public class RoomType : Audit
    {
        [Key]
        public string RoomTypeId { get; set; }

        [Display(Name = "Room Type Name")]
        [Required(ErrorMessage = "Require Room Type Name.")]
        public string RoomTypeName { get; set; }
    }
}

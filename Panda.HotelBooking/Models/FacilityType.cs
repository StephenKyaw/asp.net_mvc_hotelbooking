using System.ComponentModel.DataAnnotations;

namespace Panda.HotelBooking.Models
{
    public class FacilityType : Audit
    {
        [Key]
        public string FacilityTypeId { get; set; }

        [Display(Name = "Facility Type Name")]
        [Required(ErrorMessage = "Require Facility Type Name.")]
        public string FacilityTypeName { get; set; }
    }
}

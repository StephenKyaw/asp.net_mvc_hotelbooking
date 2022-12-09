namespace Panda.HotelBooking.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BedType : Audit
    {
        [Key]
        public string BedTypeId { get; set; }

        [Display(Name = "Bed Type Name")]
        [Required(ErrorMessage = "Require Bed Type Name.")]
        public string BedTypeName { get; set; }
    }
}

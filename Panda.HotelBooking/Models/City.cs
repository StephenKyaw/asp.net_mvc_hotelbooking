namespace Panda.HotelBooking.Models
{
    using System.ComponentModel.DataAnnotations;

    public class City : Audit
    {
        [Key]
        public string CityId { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Name.")]
        public string CityName { get; set; }

        public ICollection<Township> Townships { get; set; }
    }
}

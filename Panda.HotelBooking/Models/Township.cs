namespace Panda.HotelBooking.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Township : Audit
    {
        [Key]
        public string TownshipId { get; set; }

        [Display(Name ="Township Name")]
        [Required(ErrorMessage = "Require Township Name.")]
        public string TownshipName { get; set; }


        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Location.")]
        public string CityId { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}

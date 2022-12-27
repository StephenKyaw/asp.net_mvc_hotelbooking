using System.ComponentModel.DataAnnotations;

namespace WebMvcUI.Areas.Admin.Models
{
    public class LocationViewModel
    {
        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Name.")]
        public string CityName { get; set; }
        public string CreadedBy { get; set; }
        public string TownshipsJoson { get; set; }
        public record Townships(string Township);
    }

    public class CityViewModel : BaseViewModel
    {
        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Name.")]
        public string CityName { get; set; }
        public string Id { get; set; }
    }

    public class TownshipViewModel : BaseViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Township Name")]
        [Required(ErrorMessage = "Require Township Name.")]
        public string TownshipName { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Name.")]
        public string CityId { get; set; }

        [Display(Name = "City Name")]
        public string CityName { get; set; }
    }
}

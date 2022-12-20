using System.ComponentModel.DataAnnotations;

namespace WebMvcUI.Models
{
    public class LocationViewModel
    {
        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Name.")]
        public string CityName { get;set;}

        public string CreadedBy { get;set;}

        public List<string> Townships { get;set;} = new List<string>();

    }
}

using System.ComponentModel.DataAnnotations;

namespace WebMvcUI.Models
{
    public class HotelViewModel : BaseViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Require Hotel's Name.")]
        [Display(Name = "Hotel Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Require Hotel's Address.")]
        public string Address { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Require Phone Number.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone_1 { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone_2 { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone_3 { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Location.")]
        public string CityId { get; set; }

        [Display(Name = "City Name")]
        public string CityName { get; set; }

        [Display(Name = "TownShip Name")]
        [Required(ErrorMessage = "Require Township Location.")]
        public string TownshipId { get; set; }

        [Display(Name = "Township Name")]
        public string TownshipName { get; set; }


        [Display(Name = "Photo")]
        public IEnumerable<HotelPhotoViewModel> HotelPhotos { get; set; } = new List<HotelPhotoViewModel>();


        [Display(Name="Upload Photos")]
        public IFormFileCollection FileRoomPhotos { get;set;} = new FormFileCollection();
    }

    public class HotelPhotoViewModel
    {
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
    }
}

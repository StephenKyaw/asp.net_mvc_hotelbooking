namespace Panda.HotelBooking.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Hotel : Audit
    {
        [Key]
        public string HotelId { get; set; }

        [Required(ErrorMessage = "Require Hotel's Name.")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Require Hotel's Address.")]
        public string Address { get; set; }
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Require Phone Number.")]
        public string Phone_1 { get; set; }

        [Display(Name = "Phone")]
        public string Phone_2 { get; set; }

        [Display(Name = "Phone")]
        public string Phone_3 { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Require City Location.")]
        public string CityId { get; set; }

        public City City { get; set; }

        [Display(Name = "TownShip Name")]
        [Required(ErrorMessage = "Require Township Location.")]
        public string TownshipId { get; set; }

        public Township Township { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}

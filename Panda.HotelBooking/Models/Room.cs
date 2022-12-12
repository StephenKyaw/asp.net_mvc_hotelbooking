using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panda.HotelBooking.Models
{
    public class Room :Audit
    {
        [Key]
        public string RoomId { get; set; }

        [Display(Name = "Hotel")]
        [Required(ErrorMessage = "Require Hotel.")]
        public string HotelId { get;set;}

        [ForeignKey("HotelId")]
        public Hotel Hotel { get;set;}

        [Display(Name ="Room Type")]
        [Required(ErrorMessage ="Require Room Type.")]
        public string RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomType RoomType { get; set; }

        [Required(ErrorMessage = "Require Room Price.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public double Rate { get; set; }

        [Display(Name = "Number of Rooms")]
        [Required(ErrorMessage = "Require Number of Rooms.")]
        public int NumberOfRooms { get; set; }

        public ICollection<RoomBed> RoomBeds { get; set; }
        public ICollection<RoomPhoto> RoomPhotos { get; set; }
        public ICollection<RoomFacility> RoomFacilities { get; set; }

        [NotMapped]
        [Display(Name ="Upload Photos")]
        public IFormFileCollection FormFilePhotos { get; set;}
    }
}

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
    }
    public class RoomBed
    {
        [Key]
        public string RoomBedId { get; set; }

        public string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        [Display(Name ="Bed Type")]
        [Required(ErrorMessage = "Require Bed Type.")]
        public string BedTypeId { get; set; }

        [ForeignKey("BedTypeId")]
        public BedType BedType { get; set; }

        [Display(Name = "Number of Beds")]
        [Required(ErrorMessage = "Require Number of Beds.")]
        public int NumberOfBeds { get; set; }
    }

    public class RoomPhoto
    {
        [Key]
        public string RoomPhotoId { get;set;}

        public string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public string PhotoPath { get; set; }
    }

    public class RoomFacility
    {
        [Key]
        public string RoomFacilityId { get;set;}

        public string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public string FacilityTypeId { get; set; }

        [ForeignKey("FacilityTypeId")]
        public FacilityType FacilityType { get; set; }
    }
    //setups
    public class RoomType : Audit
    {
        [Key]
        public string RoomTypeId { get; set; }

        [Display(Name = "Room Type Name")]
        [Required(ErrorMessage = "Require Room Type Name.")]
        public string RoomTypeName { get; set; }
    }
    public class BedType : Audit
    {
        [Key]
        public string BedTypeId { get; set; }

        [Display(Name = "Bed Type Name")]
        [Required(ErrorMessage = "Require Bed Type Name.")]
        public string BedTypeName { get; set; }
    }
    public class FacilityType : Audit
    {
        [Key]
        public string FacilityTypeId { get; set; }

        [Display(Name = "Facility Type Name")]
        [Required(ErrorMessage = "Require Facility Type Name.")]
        public string FacilityTypeName { get; set; }
    }

}

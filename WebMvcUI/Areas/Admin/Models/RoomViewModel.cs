using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMvcUI.Areas.Admin.Models
{
    public class RoomViewModel : BaseViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Hotel")]
        [Required(ErrorMessage = "Require Hotel.")]
        public string HotelId { get; set; }
        public string HotelName { get; set; }

        [Display(Name = "Room Type")]
        [Required(ErrorMessage = "Require Room Type.")]
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }

        [Required(ErrorMessage = "Require Room Price.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public double Rate { get; set; }

        [Display(Name = "Number of Rooms")]
        [Required(ErrorMessage = "Require Number of Rooms.")]
        public int NumberOfRooms { get; set; }


        [Display(Name = "Upload Photos")]
        public IFormFileCollection FileRoomPhotos { get; set; } = new FormFileCollection();

        
        [Display(Name = "Add Beds.")]
        [Required(ErrorMessage = "Require Beds.")]
        public string RoomBedsJsonString { get; set; }

        public List<RoomPhotoViewModel> RoomPhotos { get; set; } = new List<RoomPhotoViewModel>();
        public List<RoomBedViewModel> RoomBeds { get; set; } = new List<RoomBedViewModel>();
        public List<SelectListItem> RoomFacilities { get; set; } = new List<SelectListItem>();
    }

    public class JsonDataItem
    {
        public string text { get; set; }
        public string value { get; set; }
    }

    public class RoomBedViewModel
    {
        public List<JsonDataItem> BedTypes { get; set; } = new List<JsonDataItem>();
        public string NumberOfBeds { get; set; }
    }

    public class RoomPhotoViewModel
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public string RoomId { get; set; }
    }
    public class RoomFacilityViewModel
    {
        public string RoomFacilityId { get; set; }
        public string FacilityTypeId { get; set; }
        public string RoomId { get; set; }
    }

    //code setups view models
    public class BedTypeViewModel : BaseViewModel
    {
        public string BedTypeId { get; set; }

        [Display(Name = "Bed Type")]
        [Required(ErrorMessage = "Require Bed Type.")]
        public string BedTypeName { get; set; }
    }
    public class FacilityTypeViewModel : BaseViewModel
    {
        public string FacilityTypeId { get; set; }

        [Display(Name = "Facility Type Name")]
        [Required(ErrorMessage = "Require Facility Type Name.")]
        public string FacilityTypeName { get; set; }
    }
    public class RoomTypeViewModel : BaseViewModel
    {
        public string RoomTypeId { get; set; }

        [Display(Name = "Room Type Name")]
        [Required(ErrorMessage = "Require Room Type Name.")]
        public string RoomTypeName { get; set; }
    }
}

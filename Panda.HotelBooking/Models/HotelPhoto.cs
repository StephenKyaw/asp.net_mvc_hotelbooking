using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panda.HotelBooking.Models
{
    public class HotelPhoto
    {
        [Key]
        public string HotelPhotoId { get;set;}

        public string HotelId { get;set;}

        [ForeignKey("HotelId")]
        public Hotel Hotel { get;set;}
        public string FileName { get;set;}
        public string OriginalFileName { get;set;}
        public string ContentType { get;set;}
    }
}

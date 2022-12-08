namespace Panda.HotelBooking.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Audit
    {
        [ScaffoldColumn(false)]
        [Display(Name ="Created User")]
        public string CreatedUserId { get; set; }

        [ForeignKey("CreatedUserId")]
        public IdentityUser CreatedUser { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        
        [ScaffoldColumn(false)]
        [Display(Name = "Updated User")]
        public string UpdatedUserId { get; set; }
        
        [ForeignKey("UpdatedUserId")]
        public IdentityUser UpdatedUser { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [ScaffoldColumn(false)]
        public bool IsActive { get; set; }

        [ScaffoldColumn(false)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}

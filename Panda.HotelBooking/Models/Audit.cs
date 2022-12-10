namespace Panda.HotelBooking.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Audit
    {
        [ScaffoldColumn(false)]
        [Display(Name ="Created User")]
        public Guid? CreatedUserId { get; set; }

        [ForeignKey("CreatedUserId")]
        public ApplicationUser CreatedUser { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        
        [ScaffoldColumn(false)]
        [Display(Name = "Updated User")]
        public Guid? UpdatedUserId { get; set; }
        
        [ForeignKey("UpdatedUserId")]
        public ApplicationUser UpdatedUser { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [ScaffoldColumn(false)]
        public bool IsActive { get; set; } = true;

        [ScaffoldColumn(false)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}

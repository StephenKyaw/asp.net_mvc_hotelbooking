using System.ComponentModel.DataAnnotations;

namespace WebMvcUI.Areas.Admin.Models
{
    public abstract class BaseViewModel
    {
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Created User")]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModified Date")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(Name = "LastModified User")]
        public string LastModifiedBy { get; set; }
    }
}

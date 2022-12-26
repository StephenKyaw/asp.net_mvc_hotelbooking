using System.ComponentModel.DataAnnotations;

namespace WebMvcUI.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Require email address.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Require password.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Require full name.")]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Require email address.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Require password.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

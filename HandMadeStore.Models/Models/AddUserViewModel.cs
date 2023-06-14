using Identity.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class AddUserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Enter City")]
        public string City { get; set; }

        [Required, Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required, Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Required]
        public string ConfirmPassword { get; set; }

        public List<RolesViewModels> Roles { get; set; }
    }
}
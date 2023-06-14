using System.ComponentModel.DataAnnotations;

namespace Identity.Models.DTOs
{
    public class ProfileFormViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
    }
}
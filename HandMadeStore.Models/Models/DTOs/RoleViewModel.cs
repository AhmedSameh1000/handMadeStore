using System.ComponentModel.DataAnnotations;

namespace Identity.Models.DTOs
{
    public class RoleViewModel
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace HandMadeStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Category name"), StringLength(50), Display(Name = "Category Name")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
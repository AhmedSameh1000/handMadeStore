using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter product name"), StringLength(50), Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter product name"), StringLength(50), Display(Name = "Product Arabic Name")]
        public string arabicName { get; set; }

        [Required(ErrorMessage = "Enter description"), StringLength(300)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Enter description"), StringLength(300), Display(Name = "Product Arabic Description")]
        public string arabicDescription { get; set; }

        [Required(ErrorMessage = "Enter price"), Range(1, Double.PositiveInfinity)]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Enter price10plus"), Range(1, Double.PositiveInfinity)]
        [Display(Name = "Price for 11 - 30")]
        public double? Price10Plus { get; set; }

        [Required(ErrorMessage = "Enter price30plus"), Range(1, Double.PositiveInfinity)]
        [Display(Name = "Price for 30+")]
        public double? Price30Plus { get; set; }

        [Required, Display(Name = "Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required, Display(Name = "Brand")]
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        [DisplayName("Image")]
        public string ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
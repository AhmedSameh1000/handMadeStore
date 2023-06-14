using Identity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeStore.Models.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [ValidateNever]
        public virtual Product Product { get; set; }

        [Range(1, int.MaxValue)]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
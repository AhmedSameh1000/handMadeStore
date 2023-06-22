using Identity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.Models.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string ReviewText { get; set; }

        public DateTime? ReviewDate { get; set; }
        public int? ProductId { get; set; }

        [ValidateNever]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
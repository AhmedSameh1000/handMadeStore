using HandMadeStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HandmadeStore.Models.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> BrandList { get; set; }
    }
}
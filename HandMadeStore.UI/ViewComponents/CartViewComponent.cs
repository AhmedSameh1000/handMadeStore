using HandMadeStore.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HandMadeStore.UI.ViewComponants
{
    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.FromResult(0);
            return View(HttpContext.Session.GetInt32(SD.CartSession));
        }
    }
}
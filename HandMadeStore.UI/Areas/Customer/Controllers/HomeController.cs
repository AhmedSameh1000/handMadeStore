using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models.Models;
using HandMadeStore.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HandMadeStore.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.Product.GetAll());
        }

        public IActionResult Details(int productId)
        {
            var CartItem = new CartItem()
            {
                ProductId = productId,
                Count = 1,
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId),
            };
            return View(CartItem);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(CartItem cartItem)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cartItem.ApplicationUserId = UserId;

            var CartItemFromDb = _unitOfWork.CartItem
                .GetFirstOrDefault(
                c => c.ApplicationUserId == cartItem.ApplicationUserId
                && c.ProductId == cartItem.ProductId);

            if (CartItemFromDb == null)
            {
                _unitOfWork.CartItem.Add(cartItem);
            }
            else
            {
                _unitOfWork.CartItem.Increment(CartItemFromDb, cartItem.Count);
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models.Models;
using HandMadeStore.Models.Models.DTOs;
using HandMadeStore.UI.Hubs;
using HandMadeStore.UI.Models;
using HandMadeStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;

namespace HandMadeStore.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ReviewsHub> _reviewsHub;
        private readonly IHubContext<MessageHub> _messageHub;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork,
            IHubContext<ReviewsHub> ReviewsHub, IHubContext<MessageHub> MessageHub)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _reviewsHub = ReviewsHub;
            _messageHub = MessageHub;
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetInt32(SD.CartSession, _unitOfWork.CartItem.GetPiecesCount());
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
            TempData["product_id"] = productId;
            ViewData["Reviews"] = _unitOfWork.Review.GetAll(r => r.ProductId == productId);
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
            HttpContext.Session.SetInt32(SD.CartSession, _unitOfWork.CartItem.GetPiecesCount());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddReview(Review review)
        {
            if (review is null || review.ReviewText is null)
            {
                ModelState.AddModelError("ReviewText", "Review Cant Be Empty");
            }
            if (!string.IsNullOrEmpty(review.ReviewText))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                review.ApplicationUserId = userId;
                review.ProductId = (int)TempData["product_id"];
                review.ReviewDate = DateTime.Now;
                _unitOfWork.Review.Add(review);
                _unitOfWork.Save();
                _reviewsHub.Clients.All.SendAsync("LoadReviews", review.ProductId);

                return RedirectToAction("Details", new { productId = review.ProductId });
            }
            return RedirectToAction("Details", new { productId = review.ProductId });
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Send(MessageVM messageVM)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Sender = _unitOfWork.ApplicationUser.GetFirstOrDefault(r => r.Id == UserId);
            var Reciver = _unitOfWork.ApplicationUser.GetFirstOrDefault(c => c.Email == "ahmeds141000@gmail.com");
            var MessageToSend = new { Sender = Sender.Name, body = messageVM.MessageText };
            _messageHub.Clients.User(Reciver.Id).SendAsync("ReciveMessage", MessageToSend);
            return RedirectToAction("Index");
        }

        public IActionResult SetCulture(string lang, string ReturnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
       CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
       new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) });

            return LocalRedirect(ReturnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
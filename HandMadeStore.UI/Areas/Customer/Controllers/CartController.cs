using HandMadeStore.DataAccess.Repository;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models.Models;
using HandMadeStore.Models.Models.DTOs;
using HandMadeStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace HandMadeStore.UI.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    [BindProperties]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public CartVM CartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartVM = new CartVM()
            {
                cartItems = _unitOfWork.CartItem.GetAll(c => c.ApplicationUserId == userId)
            };
            foreach (var item in CartVM.cartItems)
            {
                item.Price = GetCartPrice(item.Count, item.Product.Price, item.Product.Price10Plus, item.Product.Price30Plus);
                CartVM.CartTotal += (item.Price * item.Count);
                CartVM.PiecesCount += item.Count;
            }
            return View(CartVM);
        }

        public IActionResult Summary()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartVM = new CartVM()
            {
                cartItems = _unitOfWork.CartItem.GetAll(c => c.ApplicationUserId == userId),
                OrderHeader = new()
            };
            CartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                .GetFirstOrDefault(u => u.Id == userId);
            CartVM.OrderHeader.Name = CartVM.OrderHeader.ApplicationUser.Name;
            CartVM.OrderHeader.PhoneNumber = CartVM.OrderHeader.ApplicationUser.PhoneNumber;
            CartVM.OrderHeader.StreetAddress = CartVM.OrderHeader.ApplicationUser.StreetAddress;
            CartVM.OrderHeader.City = CartVM.OrderHeader.ApplicationUser.City;
            CartVM.OrderHeader.PostalCode = CartVM.OrderHeader.ApplicationUser.PostalCode;
            foreach (var item in CartVM.cartItems)
            {
                item.Price = GetCartPrice(item.Count, item.Product.Price, item.Product.Price10Plus, item.Product.Price30Plus);
                CartVM.CartTotal += (item.Price * item.Count);
                CartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
                CartVM.PiecesCount += item.Count;
            }
            return View(CartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartVM.cartItems = _unitOfWork.CartItem.GetAll(c => c.ApplicationUserId == userId);
            CartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            CartVM.OrderHeader.OrderStatus = SD.StatusPending;
            CartVM.OrderHeader.OrderDate = DateTime.Now;
            CartVM.OrderHeader.ApplicationUserId = userId;
            foreach (var item in CartVM.cartItems)
            {
                item.Price = GetCartPrice(item.Count, item.Product.Price, item.Product.Price10Plus, item.Product.Price30Plus);
                CartVM.CartTotal += (item.Price * item.Count);
                CartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
                CartVM.PiecesCount += item.Count;
            }
            _unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var item in CartVM.cartItems)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = item.ProductId,
                    OrderId = CartVM.OrderHeader.Id,
                    Price = item.Price,
                    Count = item.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
            var Domain = "https://localhost:7072/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                PaymentMethodTypes = new List<string>()
                {
                    "card"
                },
                SuccessUrl = Domain + $"Customer/cart/OrderConfirmation?id={CartVM.OrderHeader.Id}",
                CancelUrl = Domain + $"Customer/Cart/Index",
            };

            foreach (var item in CartVM.cartItems)
            {
                {
                    var SessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(SessionLineItem);
                }
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateOrderPayment(CartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);

            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation(int id)
        {
            var OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id);
            var service = new SessionService();
            Session session = service.Get(OrderHeader.SessionId);
            if (session.PaymentStatus == "paid")
            {
                _unitOfWork.OrderHeader.UpdateOrderPayment(OrderHeader.Id, session.Id, session.PaymentIntentId);

                _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }
            _emailSender.SendEmailAsync(OrderHeader.ApplicationUser.Email, "New Order Hand Made Store", $"<h2>Order Number #{OrderHeader.Id}... Total : {OrderHeader.OrderTotal} EGP </h2>");
            var CartItems = _unitOfWork.CartItem.GetAll(c => c.ApplicationUserId == OrderHeader.ApplicationUserId).ToList();
            _unitOfWork.CartItem.RemoveRange(CartItems);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.CartSession, 0);

            return View(id);
        }

        public IActionResult Increment(int CartId)
        {
            var CartItem = _unitOfWork.CartItem.GetFirstOrDefault(c => c.Id == CartId);
            _unitOfWork.CartItem.Increment(CartItem, 1);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.CartSession, _unitOfWork.CartItem.GetPiecesCount());

            return RedirectToAction("Index");
        }

        public IActionResult Decrement(int CartId)
        {
            var CartItem = _unitOfWork.CartItem.GetFirstOrDefault(c => c.Id == CartId);
            if (CartItem.Count <= 1)
            {
                _unitOfWork.CartItem.Remove(CartItem);
            }
            else
            {
                _unitOfWork.CartItem.Decrement(CartItem, 1);
            }
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.CartSession, _unitOfWork.CartItem.GetPiecesCount());

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int CartId)
        {
            var CartItem = _unitOfWork.CartItem.GetFirstOrDefault(c => c.Id == CartId);
            _unitOfWork.CartItem.Remove(CartItem);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.CartSession, _unitOfWork.CartItem.GetPiecesCount());
            return RedirectToAction("Index");
        }

        private double GetCartPrice(int count, double? Price, double? Price10Plus, double? price30plus)
        {
            if (count <= 10)
            {
                return Convert.ToDouble(Price);
            }
            else
            {
                if (count <= 30)
                {
                    return Convert.ToDouble(Price10Plus);
                }
                else
                {
                    return Convert.ToDouble(price30plus);
                }
            }
        }
    }
}

//https://dashboard.stripe.com/test/payments
//https://stripe.com/docs/testing
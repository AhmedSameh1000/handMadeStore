using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using HandMadeStore.Models.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HandMadeStore.DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartItemRepository(ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Decrement(CartItem cartItem, int amount)
        {
            cartItem.Count -= amount;
        }

        public int GetPiecesCount()
        {
            int PiecesCount = 0;
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _context.CartItems.Where
                (c => c.ApplicationUserId == userId).ToList();

            if (cartItems != null)
            {
                foreach (var item in cartItems)
                {
                    PiecesCount += item.Count;
                }
            }
            return PiecesCount;
        }

        public void Increment(CartItem cartItem, int amount)
        {
            cartItem.Count += amount;
        }
    }
}
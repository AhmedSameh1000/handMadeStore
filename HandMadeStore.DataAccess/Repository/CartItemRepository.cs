using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using HandMadeStore.Models.Models;

namespace HandMadeStore.DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Decrement(CartItem cartItem, int amount)
        {
            cartItem.Count -= amount;
        }

        public void Increment(CartItem cartItem, int amount)
        {
            cartItem.Count += amount;
        }
    }
}
using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using HandMadeStore.Models.Models;

namespace HandMadeStore.DataAccess.Repository
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        private readonly ApplicationDbContext _context;

        public ShopRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Shop shop)
        {
            _context.Update(shop);
        }
    }
}
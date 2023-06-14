using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;

namespace HandMadeStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category { get; private set; }
        public IBrandRepository Brand { get; private set; }

        public IProductRepository Product { get; private set; }

        public IShopRepository Shop { get; private set; }

        public ICartItemRepository CartItem { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Brand = new BrandRepository(context);
            Product = new ProductRepository(context);
            Shop = new ShopRepository(context);
            CartItem = new CartItemRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetail = new OrderDetailRepository(context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;

namespace HandMadeStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var ProductTOUpdate = _context.Products.Find(product.Id);
            if (ProductTOUpdate != null)
            {
                ProductTOUpdate.Name = product.Name;
                ProductTOUpdate.Description = product.Description;
                ProductTOUpdate.Price = product.Price;
                ProductTOUpdate.Price10Plus = product.Price10Plus;
                ProductTOUpdate.Price30Plus = product.Price30Plus;
                ProductTOUpdate.BrandId = product.BrandId;
                ProductTOUpdate.CategoryId = product.CategoryId;
                ProductTOUpdate.CreatedDate = product.CreatedDate;
                if (product.ImageUrl != null)
                {
                    ProductTOUpdate.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
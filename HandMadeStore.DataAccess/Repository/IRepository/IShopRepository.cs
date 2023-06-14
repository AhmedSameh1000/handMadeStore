using HandMadeStore.Models.Models;

namespace HandMadeStore.DataAccess.Repository.IRepository
{
    public interface IShopRepository : IRepository<Shop>
    {
        void Update(Shop shop);
    }
}
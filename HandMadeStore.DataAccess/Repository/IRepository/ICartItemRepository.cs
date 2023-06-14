using HandMadeStore.Models;
using HandMadeStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.DataAccess.Repository.IRepository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        void Increment(CartItem cartItem, int amount);

        void Decrement(CartItem cartItem, int amount);
    }
}
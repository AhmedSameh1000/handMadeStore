using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.Models.Models.DTOs
{
    public class CartVM
    {
        public IEnumerable<CartItem> cartItems { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public double CartTotal { get; set; }
        public int PiecesCount { get; set; }
    }
}
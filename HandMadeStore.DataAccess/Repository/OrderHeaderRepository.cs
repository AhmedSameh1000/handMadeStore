using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using HandMadeStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.Update(orderHeader);
        }

        public void UpdateOrderPayment(int id, string SessionId, string paymentIntentId)
        {
            var OrderHeader = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            OrderHeader.PaymentDate = DateTime.Now;
            OrderHeader.SessionId = SessionId;
            OrderHeader.PaymentIntentId = paymentIntentId;
        }

        public void UpdateStatus(int id, string OrderStatus, string PaymentStatus = null)
        {
            var orderHeader = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (orderHeader != null)
            {
                orderHeader.OrderStatus = OrderStatus;
                if (PaymentStatus != null)
                {
                    orderHeader.PaymentStatus = PaymentStatus;
                }
            }
        }
    }
}
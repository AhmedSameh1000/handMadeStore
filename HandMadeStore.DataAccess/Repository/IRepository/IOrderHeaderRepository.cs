using HandMadeStore.Models;
using HandMadeStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);

        void UpdateStatus(int id, string OrderStatus, string PaymentStatus = null);

        void UpdateOrderPayment(int id, string SessionId, string paymentIntentId);
    }
}
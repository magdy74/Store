using Store.Magdy.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
        Task<IEnumerable<Order>?> GetOrdersOfSpecificUserAsync(string buyerEmail);
        Task<Order?> GetOrderOfSpecificUserAsync(string buyerEmail, int orderId);
    }
}

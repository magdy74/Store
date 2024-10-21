using Store.Magdy.Core.Dtos.Baskets;
using Store.Magdy.Core.Entities;
using Store.Magdy.Core.Entities.Order;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);

        Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag);

    }
}

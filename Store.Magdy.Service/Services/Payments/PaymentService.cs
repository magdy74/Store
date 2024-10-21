using Microsoft.Extensions.Configuration;
using Store.Magdy.Core;
using Store.Magdy.Core.Dtos.Baskets;
using Store.Magdy.Core.Entities;
using Store.Magdy.Core.Entities.Order;
using Store.Magdy.Core.Services.Contract;
using Store.Magdy.Core.Specifications.Orders;
using Store.Magdy.Service.Services.Baskets;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Magdy.Core.Entities.Product;

namespace Store.Magdy.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {

            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            var basket = await _basketService.GetBasketAsync(basketId);

            if (basket is null) return null;

            var service = new PaymentIntentService();

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            if(basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
                
            }

            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);

            PaymentIntent paymentIntent;

            if (String.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                    PaymentMethodTypes = new List<string>() { "card" },
                    Currency = "usd"
                };
                
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                //Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100)
                };

                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            
            basket = await _basketService.UpdateBasketAsync(basket);

            if (basket is null) return null;

            return basket;
            

        }

        public async Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderSpecificationWithPaymentIntentId(paymentIntentId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecsAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }

            _unitOfWork.Repository<Order, int>().Update(order);

            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}

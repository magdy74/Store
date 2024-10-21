using Store.Magdy.Core;
using Store.Magdy.Core.Entities;
using Store.Magdy.Core.Entities.Order;
using Store.Magdy.Core.Repositories.Contract;
using Store.Magdy.Core.Services.Contract;
using Store.Magdy.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketService basketService, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketService.GetBasketAsync(basketId);

            if (basket is null) return null;

            var orderItems = new List<OrderItem>();

            if(basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);

                    var ProductOrderedItem = new ProductItemOrder() { ProductId = product.Id, ProductName = product.Name, PictureUrl = product.PictureUrl};
                    
                    var orderItem = new OrderItem(ProductOrderedItem, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }
            
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

            var subTotal = orderItems.Sum(I => I.Price * I.Quantity);

            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
                var ExOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecsAsync(spec);
                _unitOfWork.Repository<Order, int>().Delete(ExOrder);
            }

            var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basketDto.PaymentIntentId);

            await _unitOfWork.Repository<Order, int>().AddAsync(order);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return order;
        }

        public async Task<Order?> GetOrderOfSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);

            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecsAsync(spec);

            if(order is null) return null;

            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrdersOfSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);

            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecsAsync(spec);

            if (orders is null) return null;

            return orders;
        }
    }
}

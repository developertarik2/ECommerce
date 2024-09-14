using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Models.Entities;
using ECommerce.Models.Entities.OrderAggregate;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = ECommerce.Models.Entities.OrderAggregate.Order;

namespace ECommerce.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
       // private readonly IBasketRepository _basketRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public PaymentService(ICartRepository cartRepository, /*IBasketRepository basketRepository,*/ IUnitOfWork unitOfWork, IConfiguration config)
        {
            _config = config;
            //  _basketRepository = basketRepository;
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerCart> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            // var basket = await _basketRepository.GetBasketAsync(basketId); //with redis


            var basket = await _cartRepository.GetCartAsync(basketId);
            if (basket == null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.DeliveryMethod
                    .Get((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Product.Get(item.Id);
                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }
            var service = new PaymentIntentService();

            PaymentIntent intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            //  await _basketRepository.UpdateBasketAsync(basket); //with redis

            await _cartRepository.UpdateCartAsync(basket);
            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
          //  var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Order.GetFirstOrDefault(o => o.PaymentIntentId == paymentIntentId);

            if (order == null) return null;

            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.Save();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
           // var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Order.GetFirstOrDefault(o => o.PaymentIntentId == paymentIntentId);

            if (order == null) return null;

            order.Status = OrderStatus.PaymentRecevied;
            // _unitOfWork.Order.Add(order);
           // _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Save();

            return order;
        }
    }
}

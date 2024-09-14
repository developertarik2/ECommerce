using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Models.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
  public  class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        //  private readonly IGenericRepository<Order> _order;
        // private readonly IGenericRepository<Product> _productRepo;
       // private readonly IBasketRepository _basketRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IPaymentService _paymentService;
        //  private readonly IGenericRepository<DeliveryMethod> _dmRepo;

        public OrderService(IUnitOfWork unitOfWork,ICartRepository cartRepo /*IBasketRepository basketRepo*/, IPaymentService paymentService)
        {
            //  _order = order;
            //   _productRepo = productRepo;
            _unitOfWork = unitOfWork;
            // _basketRepo = basketRepo;
            _cartRepo = cartRepo;
            _paymentService = paymentService;
            //  _dmRepo = dmRepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //  var basket = await _basketRepo.GetBasketAsync(basketId); with redis

            var basket = await _cartRepo.GetCartAsync(basketId);
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Product.Get(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            var deliveryMethod = await _unitOfWork.DeliveryMethod.Get(deliveryMethodId);
            var subtotal = items.Sum(item => item.Price * item.Quantity);

           // var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Order.GetFirstOrDefault(o => o.PaymentIntentId == basket.PaymentIntentId);

            if (existingOrder != null)
            {
                _unitOfWork.Order.Remove(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId);
            _unitOfWork.Order.Add(order);

            var result =await  _unitOfWork.Save();
            if (result <= 0) return null;
            // await _basketRepo.DeleteBasketAsync(basketId);
            return order;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.DeliveryMethod.GetAll();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
          //  var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Order.GetFirstOrDefault(u=>u.Id==id && u.BuyerEmail==buyerEmail,includeProperties: "OrderItems,DeliveryMethod");
        }

        public async Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
           // var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Order.GetAll(u=> u.BuyerEmail==buyerEmail ,orderBy:u=>u.OrderByDescending(u=>u.OrderDate), includeProperties: "OrderItems,DeliveryMethod");
        }
    }
}

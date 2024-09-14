using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
    public class CartRepository : ICartRepository //Without Redis
    {
        private readonly ECommerceContext _context;
        public CartRepository(ECommerceContext context)
        {
            _context = context;
        }
        public Task<CustomerCart> CreateCartAsync(CustomerCart basket)
        {
            //foreach(var item in basket.Items)
            //{
            //  await  _context.CartItems.AddAsync(item);
            //}
            //  await _context.CustomerCarts.AddAsync(basket);
            //  await _context.SaveChangesAsync();
            //return await GetCartAsync(basket.Id);

            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCartAsync(string basketId)
        {
            var obj = await _context.CustomerCarts.Include(u => u.Items).FirstOrDefaultAsync(u => u.Id == basketId);
            var items = await _context.CartItems.Where(u => u.CustomerCart == obj).ToListAsync();

            _context.CartItems.RemoveRange(items);
            _context.CustomerCarts.Remove(obj);
          var done=  await _context.SaveChangesAsync();
            return done != 0;
           // throw new NotImplementedException();
        }

        public async Task< CustomerCart> GetCartAsync(string basketId)
        {
            return await _context.CustomerCarts.Include(u=>u.Items).FirstOrDefaultAsync(u => u.Id == basketId);
         //  return JsonSerializer.Deserialize<CustomerCart>(cart);
            // throw new NotImplementedException();
        }

        public async Task<CustomerCart> UpdateCartAsync(CustomerCart basket)
        {
            var obj =await _context.CustomerCarts.Include(u=>u.Items).FirstOrDefaultAsync(u => u.Id == basket.Id);
            if(obj==null)
            {
               await _context.CustomerCarts.AddAsync(basket);
                foreach(var item in basket.Items)
                {
                    item.CustomerCart = basket;
                    await _context.CartItems.AddAsync(item);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                List<CartItem> oldItems = new();
                 oldItems =  _context.CartItems.Where(u => u.CustomerCart == obj).ToList();
                List<CartItem> newItems = new();
                newItems = basket.Items;

                foreach(var cartItem in newItems)
                {
                    var item = _context.CartItems.FirstOrDefault(u => u.Id == cartItem.Id && u.CustomerCart == obj);
                    if(item!=null)
                    {
                        item.Quantity = cartItem.Quantity;
                        _context.SaveChanges();
                        oldItems.Remove(item);
                    }
                    else
                    {
                        _context.CartItems.Add(new CartItem
                        {
                            Id = cartItem.Id,
                            Brand = cartItem.Brand,
                            Category = cartItem.Category,
                            CustomerCart = obj,
                            PictureUrl = cartItem.PictureUrl,
                            Price = cartItem.Price,
                            ProductName = cartItem.ProductName,
                            Quantity = cartItem.Quantity
                        });
                        _context.SaveChanges();
                    }
                }
                if(oldItems.Any())
                {
                    foreach (var cartItem in oldItems)
                    {
                        _context.CartItems.Remove(cartItem);
                        _context.SaveChanges();
                    }
                    
                }

                obj.DeliveryMethodId = basket.DeliveryMethodId;
                obj.ClientSecret = basket.ClientSecret;
                obj.PaymentIntentId = basket.PaymentIntentId;
                obj.ShippingPrice = basket.ShippingPrice;
               // _context.CustomerCarts.Update(obj);
                 _context.SaveChanges();

            }
            var newobj = await _context.CustomerCarts.Include(u => u.Items).FirstOrDefaultAsync(u => u.Id == basket.Id);
            return  newobj;
           
        }

        
    }
}

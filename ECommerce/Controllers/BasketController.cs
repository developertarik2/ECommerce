using AutoMapper;
using ECommerce.Dtos;
using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IMapper _mapper;
      //  private readonly IBasketRepository _basketRepository;

        private readonly ICartRepository _cartRepository;
        public BasketController(/*IBasketRepository basketRepository,*/IMapper mapper, ICartRepository cartRepository)
        {
           // _basketRepository = basketRepository;
            _mapper = mapper;
            _cartRepository = cartRepository;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetBasketById(string id)
        {
            //var basket =await _basketRepository.GetBasketAsync(id); //with redis

            var basket = await _cartRepository.GetCartAsync(id);
            List<CartItem> items = new List<CartItem>();
            return Ok(basket);
            //return Ok(new CustomerCart 
            //{
            //    Id= "2d75401a-4b68-4908-ba30-74c003bbb28f",
            //    ShippingPrice=0m,
            //    Items=basket.Items
            //});;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerCart>> UpdateBasket(CustomerCart basket)
        {
            //   var customerBasket = _mapper.Map<CustomerCartDto, CustomerCart>(basket);
            // var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);  //with redis

            var updatedBasket = await _cartRepository.UpdateCartAsync(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            // await _basketRepository.DeleteBasketAsync(id);

            await _cartRepository.DeleteCartAsync(id);
        }
    }
}

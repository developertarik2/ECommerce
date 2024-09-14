using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Models.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerCart> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(data);
        }

        public async Task<CustomerCart> UpdateBasketAsync(CustomerCart basket)
        {
            var created = await _database.StringSetAsync(basket.Id,
               JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!created) return null;

            return await GetBasketAsync(basket.Id);

        }
    }
}

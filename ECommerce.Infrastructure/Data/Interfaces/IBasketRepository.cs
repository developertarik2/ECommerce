using ECommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Interfaces
{
   public interface IBasketRepository
    {
        Task<CustomerCart> GetBasketAsync(string basketId);
        Task<CustomerCart> UpdateBasketAsync(CustomerCart basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
